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
                    "\tUsage:",
                    $"\t\t{command.Names[0]} {String.Join ( " ", command.Arguments.Select ( arg => GetPrettyArgumentName ( arg ) ) )}"
                };

                if ( command.Names.Length > 1 )
                    list.Add ( $"\tAliases: {String.Join ( ", ", command.Names )}" );

                if ( command.Arguments.Length > 0 )
                {
                    var maxLen = command.Arguments.Max ( arg =>
                        arg.Name.Length
                        + 1
                        + arg.ParameterType.Name.Length );
                    Console.WriteLine ( $"maxLen: {maxLen}" );
                    Console.WriteLine ( $"Args: {String.Join ( ", ", command.Arguments.Select ( arg => $"{arg.Name}:{arg.ParameterType.Name}" ) )}" );

                    list.Add ( "\tArguments:" );
                    foreach ( ArgumentHelpData argument in command.Arguments )
                    {
                        var start = $"{argument.Name}:{argument.ParameterType.Name}";
                        list.Add ( $"\t\t{start.PadRight ( maxLen, ' ' )} - {argument.Description}" );
                        if ( argument.ParameterType.IsEnum )
                            list.Add ( $"\t\t\tPossible values: {String.Join ( ", ", Enum.GetNames ( argument.ParameterType ) )}" );
                    }
                }

                if ( command.Examples.Length > 0 )
                {
                    list.Add ( $"\tExamples:" );
                    foreach ( var example in command.Examples )
                        list.Add ( $"\t\t{example}" );
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
                        this.WriteLine ( '\t' + line );
            }
        }
    }
}
