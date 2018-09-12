using System;
using System.Collections.Generic;
using System.Text;

namespace GUtils.CLI.Commands
{
    [AttributeUsage ( AttributeTargets.Method, AllowMultiple = true, Inherited = true )]
    public sealed class HelpExampleAttribute : Attribute
    {
        public readonly String Example;

        public HelpExampleAttribute ( String example )
        {
            if ( String.IsNullOrWhiteSpace ( example ) )
                throw new ArgumentException ( "Example cannot be null or whitespace.", nameof ( example ) );

            this.Example = example;
        }
    }
}
