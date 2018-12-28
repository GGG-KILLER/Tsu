using System;

namespace GUtils.CLI.Commands
{
    /// <summary>
    /// Represents a nested command (or verb, if you will)
    /// </summary>
    public class Verb
    {
        /// <summary>
        /// The command manager for this nested command
        /// </summary>
        public CommandManager Manager { get; }

        internal Verb ( CommandManager manager )
        {
            this.Manager = manager;
        }

        internal void RunCommand ( String rest ) =>
            this.Manager.Execute ( rest );
    }
}
