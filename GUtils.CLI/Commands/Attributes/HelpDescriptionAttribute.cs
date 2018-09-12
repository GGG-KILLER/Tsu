using System;

namespace GUtils.CLI.Commands
{
    [AttributeUsage ( AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true )]
    public sealed class HelpDescriptionAttribute : Attribute
    {
        public readonly String Description;

        public HelpDescriptionAttribute ( String description )
        {
            this.Description = description;
        }
    }
}
