using System;

namespace GUtils.CLI.Commands
{
    [Flags]
    public enum CommandManagerFlags
    {
        UseSimpleParsing = 0b1,
        Default = 0b0
    }
}
