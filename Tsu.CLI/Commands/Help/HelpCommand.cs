// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Tsu.CLI.Commands.Help
{
    /// <summary>
    /// The abstract help command provider class
    /// </summary>
    public abstract class HelpCommand
    {
        private readonly BaseCommandManager Manager;
        private readonly Dictionary<Command, string[]> Cache;

        /// <summary>
        /// Initializes the default help command class
        /// </summary>
        /// <param name="manager"></param>
        protected HelpCommand(BaseCommandManager manager)
        {
            Manager = manager;
            Cache = new Dictionary<Command, string[]>();
        }

        #region I/O

        /// <summary>
        /// Writes a character to the output
        /// </summary>
        /// <param name="ch"></param>
        protected abstract void Write(char ch);

        /// <summary>
        /// Writes a string to the output
        /// </summary>
        /// <param name="str"></param>
        protected abstract void Write(string str);

        /// <summary>
        /// Writes a character followed by a <see cref="Environment.NewLine" /> to the output
        /// </summary>
        /// <param name="ch"></param>
        protected abstract void WriteLine(char ch);

        /// <summary>
        /// Writes a string followed by a <see cref="Environment.NewLine" /> to the output
        /// </summary>
        /// <param name="str"></param>
        protected abstract void WriteLine(string str);

        #endregion I/O

        #region Helpers

        /// <summary>
        /// Checks whether a given command exists
        /// </summary>
        /// <param name="input"></param>
        /// <param name="command"></param>
        /// <param name="parentCommand"></param>
        /// <returns></returns>
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Applicable for some target frameworks.")]
        [SuppressMessage("Style", "IDE0057:Use range operator", Justification = "Not available on all target frameworks.")]
        protected bool TryGetCommand(string input, out Command command, IVerbCommand parentCommand = null)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException($"'{nameof(input)}' cannot be null or empty", nameof(input));

            var commandManager = parentCommand?.CommandManager ?? Manager;

#if HAS_STRING_STRINGCOMPARISON_OVERLOADS
            var spaceIdx = input.IndexOf(' ', StringComparison.Ordinal);
#else
            var spaceIdx = CultureInfo.InvariantCulture.CompareInfo.IndexOf(input, ' ', CompareOptions.Ordinal);
#endif
            var commandName = spaceIdx != -1 ? input.Substring(0, spaceIdx) : input;
            if (commandManager.CommandDictionary.TryGetValue(commandName, out command))
            {
                var subCommandName = spaceIdx != -1 ? input.Substring(spaceIdx + 1).Trim() : "";
                if (!string.IsNullOrWhiteSpace(subCommandName) && command is IVerbCommand verb)
                    return TryGetCommand(subCommandName, out command, verb);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns the name of an argument formatted in a pretty way
        /// </summary>
        /// <param name="argumentHelp"></param>
        /// <returns></returns>
        protected static string GetPrettyArgumentName(ArgumentHelpData argumentHelp)
        {
            var name = new StringBuilder();

            name.Append(argumentHelp.Name);

            if ((argumentHelp.Modifiers & (ArgumentModifiers.JoinRest | ArgumentModifiers.Params)) != 0)
                name.Append("...");

            // Both params and args with default values are optional
            if ((argumentHelp.Modifiers & (ArgumentModifiers.Optional | ArgumentModifiers.Params)) != 0)
            {
                name.Insert(0, '[');
                name.Append(']');
            }

            return name.ToString();
        }

        /// <summary>
        /// Returns the lines of help text for a given command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected virtual string[] GetHelpLines(Command command)
        {
            if (command is null)
                throw new ArgumentNullException(nameof(command));

            if (!Cache.ContainsKey(command))
            {
                var list = new List<string>
                {
                    $"{command.Names[0]} - {command.Description}",
                    "    Usage:",
                    $"        {command.Names[0]} {string.Join ( " ", command.Arguments.Select ( arg => GetPrettyArgumentName ( arg ) ) )}"
                };

                if (command.Names.Length > 1)
                    list.Add($"    Aliases: {string.Join(", ", command.Names)}");

                if (command.Arguments.Length > 0)
                {
                    var maxLen = command.Arguments.Max(arg => arg.Name.Length
                                                              + 1
                                                              + arg.ParameterType.Name.Length);

                    list.Add("    Arguments:");
                    foreach (var argument in command.Arguments)
                    {
                        var start = $"{argument.Name}:{argument.ParameterType.Name}";
                        list.Add($"        {start.PadRight(maxLen, ' ')} - {argument.Description}");
                        if (argument.ParameterType.IsEnum)
                            list.Add($"            Possible values: {string.Join(", ", Enum.GetNames(argument.ParameterType))}");
                    }
                }

                if (command.Examples.Length > 0)
                {
                    list.Add($"    Examples:");
                    foreach (var example in command.Examples)
                        list.Add($"        {example}");
                }

                Cache[command] = list.ToArray();
            }

            return Cache[command];
        }

        #endregion Helpers

        /// <summary>
        /// Shows the help for a specific command or all commands
        /// </summary>
        /// <param name="commandName"></param>
        [RawInput]
        [Command("help", Overwrite = true)]
        [HelpDescription("Shows help text")]
        [HelpExample("help      (will list all commands)")]
        [HelpExample("help help (will show the help text for this command)")]
        protected virtual void HelpCommandAction(
            [HelpDescription("name of the command to get the help text")] string commandName = null)
        {
            if (commandName != null)
            {
                if (TryGetCommand(commandName, out var command))
                {
                    if (command is IVerbCommand verb)
                    {
                        WriteLine($"Showing help for all commands of {commandName}:");
                        foreach (var subCommand in verb.CommandManager.Commands)
                        {
                            foreach (var line in GetHelpLines(subCommand))
                                WriteLine("    " + line);
                        }
                    }
                    else
                    {
                        foreach (var line in GetHelpLines(command))
                            WriteLine(line);
                    }
                }
                else
                {
                    WriteLine($"Command '{commandName}' doesn't exists.");
                }
            }
            else
            {
                WriteLine("Showing help for all commands:");
                foreach (var command in Manager.Commands)
                {
                    foreach (var line in GetHelpLines(command))
                        WriteLine("    " + line);
                }
            }
        }
    }
}