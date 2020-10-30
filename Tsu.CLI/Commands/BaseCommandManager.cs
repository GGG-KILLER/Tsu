using System;
using System.Collections.Generic;

namespace Tsu.CLI.Commands
{
    /// <summary>
    /// The base command manager class.
    /// </summary>
    public abstract class BaseCommandManager
    {
        /// <summary>
        /// The commands registered in this command manager.
        /// </summary>
        public abstract IReadOnlyList<Command> Commands { get; }

        /// <summary>
        /// The lookup table for commands based on command names.
        /// </summary>
        public abstract IReadOnlyDictionary<String, Command> CommandDictionary { get; }

        /// <summary>
        /// Executes a command from an input line.
        /// </summary>
        /// <param name="line">
        /// The line containing the command to be executed and the command's arguments.
        /// </param>
        public abstract void Execute ( String line );
    }
}