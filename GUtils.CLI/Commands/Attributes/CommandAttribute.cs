using System;

namespace GUtils.CLI.Commands
{
    [AttributeUsage ( AttributeTargets.Method, AllowMultiple = true, Inherited = true )]
    public sealed class CommandAttribute : Attribute
    {
        public String Name { get; }
        public Boolean Overwrite { get; set; }

        public CommandAttribute ( String Name )
        {
            if ( String.IsNullOrWhiteSpace ( Name ) )
                throw new ArgumentException ( "Command name must not be null or composed of whitespaces.", nameof ( Name ) );
            if ( Name.Contains ( " " ) )
                throw new ArgumentException ( "Command name cannot have whitespaces in it.", nameof ( Name ) );
            this.Name = Name;
        }
    }
}
