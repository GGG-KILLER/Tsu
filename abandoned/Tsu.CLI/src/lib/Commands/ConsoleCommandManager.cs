﻿// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Tsu.CLI.Commands.Help;

namespace Tsu.CLI.Commands
{
    /// <summary>
    /// A command manager made to be used within a console application
    /// </summary>
    public class ConsoleCommandManager : CompiledCommandManager
    {
        /// <summary>
        /// The prompt to print before asking the user for a command
        /// </summary>
        public string Prompt { get; set; } = "> ";

        /// <summary>
        /// Whether the loop of this <see cref="ConsoleCommandManager" /> is being executed
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Whether exit commands were registered with
        /// </summary>
        public bool HasExitCommand { get; private set; }

        /// <summary>
        /// <para>
        /// Event is triggered when an exception that is not part of the command loop is thrown.
        /// </para>
        /// </summary>
        public event EventHandler<Exception> CommandExecutionErrored;

        /// <summary>
        /// Initializes a console command manager
        /// </summary>
        public ConsoleCommandManager() : base()
        {
        }

        #region Nested Commands Hack

        /// <summary>
        /// Adds a nested command (or verb, if you will) to this command manager.
        /// </summary>
        /// <param name="verb"></param>
        /// <returns>The <see cref="CompiledCommandManager" /> created for the verb</returns>
        public override CompiledCommandManager AddVerb(string verb)
        {
            if (string.IsNullOrWhiteSpace(verb))
                throw new ArgumentException("Verb cannot be null, empty or contain any whitespaces.", nameof(verb));
            if (verb.Any(char.IsWhiteSpace))
                throw new ArgumentException("Verb cannot have whitespaces.", nameof(verb));
            if (CommandDictionary.ContainsKey(verb))
                throw new InvalidOperationException("A command with this name already exists.");

            // Verb creation
            var verbCommandManager = new ConsoleCommandManager();
            var verbInst = new Verb(verbCommandManager);

            // Command registering
            var command = new VerbCompiledCommand(
                verbCommandManager,
                typeof(Verb).GetMethod(nameof(Verb.RunCommand), BindingFlags.Instance | BindingFlags.Public),
                verbInst,
                new[] { verb },
                isRaw: true
            );
            CommandList.Add(command);
            CommandLookupTable[verb] = command;

            return verbInst.Manager;
        }

        #endregion Nested Commands Hack

        /// <summary>
        /// Adds the <see cref="ConsoleHelpCommand" /> to this command manager
        /// </summary>
        public void AddHelpCommand() =>
            AddHelpCommand(new ConsoleHelpCommand(this));

        #region Command Loop

        /// <summary>
        /// Registers a command to end the command loop with the provided names
        /// </summary>
        /// <param name="names">All possible names for the exit command</param>
        public void AddStopCommand(params string[] names)
        {
            if (names == null)
                throw new ArgumentNullException(nameof(names));
            if (names.Length < 1)
                throw new ArgumentException("No names provided.", nameof(names));

            var exitCommand = new CompiledCommand(
                typeof(ConsoleCommandManager).GetMethod(nameof(Stop)),
                this,
                names,
                "Exits this command loop.");
            CommandList.Add(exitCommand);
            foreach (var name in names)
                CommandLookupTable[name] = exitCommand;
            HasExitCommand = true;
        }

        private static void PrintError(string line)
        {
            var fgc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(line);
            Console.ForegroundColor = fgc;
        }

        /// <summary>
        /// Starts the command loop execution
        /// </summary>
        /// <remarks>
        /// <para>
        /// <see cref="Errors.NonExistentCommandException" />,
        /// <see cref="Errors.CommandInvocationException" /> and
        /// <see cref="Errors.InputLineParseException" /> are caught as part of the command execution loop
        /// and formatted as error messages in the console.
        /// </para>
        /// <para>
        /// Any other exceptions are silently caught and forwarded to
        /// <see cref="CommandExecutionErrored" />, if and only if there are any subscriptions to this
        /// event.
        /// </para>
        /// </remarks>
        public void Start()
        {
            if (IsRunning)
                throw new InvalidOperationException("Loop is already running");

            IsRunning = true;
            while (IsRunning)
            {
                string line = null;
                try
                {
                    Console.Write(Prompt);
                    Execute(line = Console.ReadLine());
                }
                catch (Errors.NonExistentCommandException nce)
                {
                    PrintError($"Command '{nce.Command}' does not exist.");
                }
                catch (Errors.CommandInvocationException cie)
                {
                    PrintError($"Error while executing command '{cie.Command}': {cie.Message}");
                    Debug.WriteLine(cie);
                }
                catch (Errors.InputLineParseException ipe)
                {
                    PrintError($@"Error while parsing input: {ipe.Message}
{line}
{new string(' ', ipe.Offset)}^");
                }
                catch (Exception ex) when (CommandExecutionErrored != null)
                {
                    CommandExecutionErrored?.Invoke(this, ex);
                }
            }
        }

        /// <summary>
        /// Stops the command loop execution
        /// </summary>
        public void Stop()
        {
            if (!IsRunning)
                throw new InvalidOperationException("Loop is not running");

            IsRunning = false;
        }

        #endregion Command Loop
    }
}
