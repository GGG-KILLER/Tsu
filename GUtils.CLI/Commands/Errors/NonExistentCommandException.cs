using System;
using System.Collections.Generic;
using System.Text;

namespace GUtils.CLI.Commands.Errors
{
    public class NonExistentCommandException : CommandInvocationException
    {
        public NonExistentCommandException ( String command ) : base ( command, "Command does not exist." )
        {
        }

        public NonExistentCommandException ( String command, Exception innerException ) : base ( command, "Command does not exist.", innerException )
        {
        }
    }
}
