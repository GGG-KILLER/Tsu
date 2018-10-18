/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the “Software”), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
 * the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUtils.CLI.Commands.Help
{
    public class DefaultHelpCommand
    {
        private readonly CommandManager Manager;
        private readonly Dictionary<Command, String[]> Cache;

        public DefaultHelpCommand ( in CommandManager manager )
        {
            this.Manager = manager;
            this.Cache = new Dictionary<Command, String[]> ( );
        }

        #region Write/WriteLine

        /// <summary>
        /// Prints a character
        /// </summary>
        /// <param name="ch"></param>
        protected virtual void Write ( in Char ch ) => Console.Write ( ch );

        /// <summary>
        /// Prints a string
        /// </summary>
        /// <param name="text"></param>
        protected virtual void Write ( in String text ) => Console.Write ( text );

        /// <summary>
        /// Prints a string followed by a new line
        /// </summary>
        /// <param name="line"></param>
        protected virtual void WriteLine ( in String line ) => Console.WriteLine ( line );

        #endregion Write/WriteLine

        #region Helpers

        /// <summary>
        /// Checks whether a given command exists
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        protected Boolean CommandExists ( in String commandName ) => this.Manager.CommandLookupTable.ContainsKey ( commandName );

        /// <summary>
        /// Returns the help text for a given command name
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        protected Command GetCommand ( in String commandName ) => this.Manager.CommandLookupTable[commandName];

        /// <summary>
        /// Returns the name of an argument formatted in a pretty way
        /// </summary>
        /// <param name="argumentHelp"></param>
        /// <returns></returns>
        private static String GetPrettyArgumentName ( in ArgumentHelpData argumentHelp )
        {
            var name = new StringBuilder ( argumentHelp.Name );

            if ( ( argumentHelp.Modifiers & ( ArgumentModifiers.JoinRest | ArgumentModifiers.Params ) ) != 0 )
                name.Append ( "..." );

            // Both params and args with default values are optional
            if ( ( argumentHelp.Modifiers & ( ArgumentModifiers.Optional | ArgumentModifiers.Params ) ) != 0 )
            {
                name.Insert ( 0, '[' );
                name.Append ( ']' );
            }

            return name.ToString ( );
        }

        /// <summary>
        /// Returns the lines of help text for a given command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected virtual String[] GetHelpLines ( in Command command )
        {
            if ( !this.Cache.ContainsKey ( command ) )
            {
                var list = new List<String>
                {
                    $"{command.Names[0]} - {command.Description}",
                    "    Usage:",
                    $"        {command.Names[0]} {String.Join ( " ", command.Arguments.Select ( arg => GetPrettyArgumentName ( arg ) ) )}"
                };

                if ( command.Names.Length > 1 )
                    list.Add ( $"    Aliases: {String.Join ( ", ", command.Names )}" );

                if ( command.Arguments.Length > 0 )
                {
                    var maxLen = command.Arguments.Max ( arg =>
                        arg.Name.Length
                        + 1
                        + arg.ParameterType.Name.Length );

                    list.Add ( "    Arguments:" );
                    foreach ( ArgumentHelpData argument in command.Arguments )
                    {
                        var start = $"{argument.Name}:{argument.ParameterType.Name}";
                        list.Add ( $"        {start.PadRight ( maxLen, ' ' )} - {argument.Description}" );
                        if ( argument.ParameterType.IsEnum )
                            list.Add ( $"            Possible values: {String.Join ( ", ", Enum.GetNames ( argument.ParameterType ) )}" );
                    }
                }

                if ( command.Examples.Length > 0 )
                {
                    list.Add ( $"    Examples:" );
                    foreach ( var example in command.Examples )
                        list.Add ( $"        {example}" );
                }

                this.Cache[command] = list.ToArray ( );
            }

            return this.Cache[command];
        }

        #endregion Helpers

        /// <summary>
        /// Shows the help for a specific command or all commands
        /// </summary>
        /// <param name="commandName"></param>
        [Command ( "help", Overwrite = true )]
        [HelpDescription ( "Shows help text" )]
        [HelpExample ( "help      (will list all commands)" )]
        [HelpExample ( "help help (will show the help text for this command)" )]
        protected void HelpCommand (
            [HelpDescription ( "name of the command to get the help text" )] String commandName = null )
        {
            if ( commandName != null )
            {
                if ( this.CommandExists ( commandName ) )
                    foreach ( var line in this.GetHelpLines ( this.GetCommand ( commandName ) ) )
                        this.WriteLine ( line );
                else
                    this.WriteLine ( $"Command '{commandName}' doesn't exists." );
            }
            else
            {
                this.WriteLine ( "Showing help for all commands:" );
                foreach ( Command command in this.Manager.Commands )
                    foreach ( var line in this.GetHelpLines ( command ) )
                        this.WriteLine ( "    " + line );
            }
        }
    }
}