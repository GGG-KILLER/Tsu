using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GUtils.CLI.Commands.Errors
{
    public class CommandDefinitionException : Exception
    {
        public readonly MethodInfo Method;

        public CommandDefinitionException ( MethodInfo method, String message ) : base ( message )
        {
            this.Method = method;
        }

        public CommandDefinitionException ( MethodInfo method, String message, Exception innerException ) : base ( message, innerException )
        {
            this.Method = method;
        }
    }
}
