using System;

namespace GUtils.CLI.Commands
{
    [AttributeUsage ( AttributeTargets.Method, AllowMultiple = false, Inherited = true )]
    public sealed class RawInputAttribute : Attribute
    {
    }
}
