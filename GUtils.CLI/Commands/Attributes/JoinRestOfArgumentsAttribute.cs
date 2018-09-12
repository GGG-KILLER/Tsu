using System;

namespace GUtils.CLI.Commands
{
    [AttributeUsage ( AttributeTargets.Parameter, AllowMultiple = false, Inherited = true )]
    public sealed class JoinRestOfArgumentsAttribute : Attribute
    {
    }
}
