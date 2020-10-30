using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Tsu.CLI.Commands
{
    /// <summary>
    /// A compiled verb command
    /// </summary>
    public class VerbCompiledCommand : CompiledCommand, IVerbCommand
    {
        /// <inheritdoc/>
        public BaseCommandManager CommandManager { get; }

        internal VerbCompiledCommand (
            BaseCommandManager commandManager,
            MethodInfo method,
            Object instance,
            IEnumerable<String> names,
            String description = "No description provided for this command.",
            Boolean isRaw = false,
            IEnumerable<String> examples = null )
            : base ( method, instance, names, description, isRaw, examples )
        {
            this.CommandManager = commandManager ?? throw new ArgumentNullException ( nameof ( commandManager ) );
        }

        internal VerbCompiledCommand (
            BaseCommandManager commandManager,
            MethodInfo method,
            Object instance )
            : base ( method, instance )
        {
            this.CommandManager = commandManager ?? throw new ArgumentNullException ( nameof ( commandManager ) );
        }
    }
}
