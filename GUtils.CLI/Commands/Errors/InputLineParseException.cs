using System;
using System.Collections.Generic;
using System.Text;

namespace GUtils.CLI.Commands.Errors
{
    public class InputLineParseException : Exception
    {
        public readonly Int32 Offset;

        public InputLineParseException ( String message, Int32 offset ) : base ( message )
        {
            this.Offset = offset;
        }

        public InputLineParseException ( String message, Int32 offset, Exception innerException ) : base ( message, innerException )
        {
            this.Offset = offset;
        }
    }
}
