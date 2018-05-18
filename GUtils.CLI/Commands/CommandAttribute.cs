using System;

namespace GUtils.CLI.Commands
{
    [AttributeUsage ( AttributeTargets.Method, Inherited = true, AllowMultiple = true )]
    public sealed class CommandAttribute : Attribute
    {
        public String Name { get; }

        public CommandAttribute ( String Name )
        {
            this.Name = Name;
        }
    }
}
