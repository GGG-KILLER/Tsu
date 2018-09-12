using System;
using System.Collections.Generic;
using System.Text;

namespace GUtils.CLI.Commands.Errors
{
    public class CommandInvocationException : Exception
    {
        public readonly String Command;

        public CommandInvocationException ( String command, String message ) : base ( message )
        {
            this.Command = command;
        }

        public CommandInvocationException ( String command, String message, Exception innerException ) : base ( message, innerException )
        {
            this.Command = command;
        }
    }
}
