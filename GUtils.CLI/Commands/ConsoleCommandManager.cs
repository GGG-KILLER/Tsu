using System;
using GUtils.CLI.Commands.Help;

namespace GUtils.CLI.Commands
{
    /// <summary>
    /// A command manager made to be used within a console application
    /// </summary>
    public class ConsoleCommandManager : CommandManager
    {
        /// <summary>
        /// The prompt to print before asking the user for a command
        /// </summary>
        public String Prompt { get; } = "> ";

        /// <summary>
        /// Whether the loop of this <see cref="ConsoleCommandManager" /> is being executed
        /// </summary>
        public Boolean IsRunning { get; private set; }

        /// <summary>
        /// Whether exist commands were registered with
        /// </summary>
        public Boolean HasExitCommand { get; private set; }

        /// <summary>
        /// Initializes a console command manager
        /// </summary>
        public ConsoleCommandManager ( ) : base ( )
        {
        }

        /// <summary>
        /// Adds the <see cref="ConsoleHelpCommand" /> to this command manager
        /// </summary>
        public void AddHelpCommand ( ) =>
            this.AddHelpCommand ( new ConsoleHelpCommand ( this ) );

        /// <summary>
        /// Registers a command to end the command loop with the provided names
        /// </summary>
        /// <param name="names">All possible names for the exit command</param>
        public void AddStopCommand ( params String[] names )
        {
            if ( names == null )
                throw new ArgumentNullException ( nameof ( names ) );
            if ( names.Length < 1 )
                throw new ArgumentException ( "No names provided.", nameof ( names ) );

            var exitCommand = new Command (
                typeof ( ConsoleCommandManager ).GetMethod ( nameof ( Stop ) ),
                this,
                names,
                "Exits this command loop." );
            this.CommandList.Add ( exitCommand );
            foreach ( var name in names )
                this.CommandLookupTable[name] = exitCommand;
            this.HasExitCommand = true;
        }

        /// <summary>
        /// Starts the command loop execution
        /// </summary>
        public void Start ( )
        {
            if ( this.IsRunning )
                throw new InvalidOperationException ( "Loop is already running" );

            this.IsRunning = true;
            while ( this.IsRunning )
            {
                Console.Write ( this.Prompt );
                this.Execute ( Console.ReadLine ( ) );
            }
        }

        /// <summary>
        /// Stops the command loop execution
        /// </summary>
        public void Stop ( )
        {
            if ( !this.IsRunning )
                throw new InvalidOperationException ( "Loop is not running" );

            this.IsRunning = false;
        }
    }
}
