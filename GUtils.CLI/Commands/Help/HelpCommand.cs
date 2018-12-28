using System;
using System.Collections.Generic;
using System.Linq;
using GUtils.Pooling;

namespace GUtils.CLI.Commands.Help
{
    /// <summary>
    /// The abstract help command provider class
    /// </summary>
    public abstract class HelpCommand
    {
        private readonly CommandManager Manager;
        private readonly Dictionary<Command, String[]> Cache;

        /// <summary>
        /// Initializes the default help command class
        /// </summary>
        /// <param name="manager"></param>
        protected HelpCommand ( CommandManager manager )
        {
            this.Manager = manager;
            this.Cache = new Dictionary<Command, String[]> ( );
        }

        #region I/O

        /// <summary>
        /// Writes a character to the output
        /// </summary>
        /// <param name="ch"></param>
        protected abstract void Write ( Char ch );

        /// <summary>
        /// Writes a string to the output
        /// </summary>
        /// <param name="str"></param>
        protected abstract void Write ( String str );

        /// <summary>
        /// Writes a character followed by a <see cref="Environment.NewLine" /> to the output
        /// </summary>
        /// <param name="ch"></param>
        protected abstract void WriteLine ( Char ch );

        /// <summary>
        /// Writes a string followed by a <see cref="Environment.NewLine" /> to the output
        /// </summary>
        /// <param name="str"></param>
        protected abstract void WriteLine ( String str );

        #endregion I/O

        #region Helpers

        /// <summary>
        /// Checks whether a given command exists
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        protected Boolean CommandExists ( String commandName ) =>
            this.Manager.CommandLookupTable.ContainsKey ( commandName );

        /// <summary>
        /// Returns the help text for a given command name
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        protected Command GetCommand ( String commandName ) =>
            this.Manager.CommandLookupTable[commandName];

        /// <summary>
        /// Returns the name of an argument formatted in a pretty way
        /// </summary>
        /// <param name="argumentHelp"></param>
        /// <returns></returns>
        protected static String GetPrettyArgumentName ( ArgumentHelpData argumentHelp ) =>
            StringBuilderPool.Shared.WithRentedItem ( name =>
            {
                name.Append ( argumentHelp.Name );

                if ( ( argumentHelp.Modifiers & ( ArgumentModifiers.JoinRest | ArgumentModifiers.Params ) ) != 0 )
                    name.Append ( "..." );

                // Both params and args with default values are optional
                if ( ( argumentHelp.Modifiers & ( ArgumentModifiers.Optional | ArgumentModifiers.Params ) ) != 0 )
                {
                    name.Insert ( 0, '[' );
                    name.Append ( ']' );
                }

                return name.ToString ( );
            } );

        /// <summary>
        /// Returns the lines of help text for a given command
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected virtual String[] GetHelpLines ( Command command )
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
        protected virtual void HelpCommandAction (
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
