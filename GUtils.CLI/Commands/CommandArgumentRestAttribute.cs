using System;

namespace GUtils.CLI.Commands
{
    [AttributeUsage ( AttributeTargets.Parameter, AllowMultiple = false, Inherited = false )]
    internal sealed class CommandArgumentRestAttribute : Attribute
    {
    }
}
