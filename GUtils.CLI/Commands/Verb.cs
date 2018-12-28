using System;

namespace GUtils.CLI.Commands
{
    /// <summary>
    /// Represents a verb
    /// </summary>
    internal class Verb
    {
        public CommandManager Manager { get; }

        public Verb ( CommandManager manager )
        {
            this.Manager = manager;
        }

        public void RunCommand ( String rest ) =>
            this.Manager.Execute ( rest );
    }
}
