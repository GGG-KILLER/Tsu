using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace GUtils.CLI.SourceGenerator.CommandManager
{
    internal readonly struct Error
    {
        public ErrorKind ErrorKind { get; }
        public Location Location { get; }
        public ImmutableArray<Object> FormatParams { get; }

        public Error ( ErrorKind errorKind, Location location, params Object[] formatParams )
        {
            this.ErrorKind = errorKind;
            this.Location = location;
            this.FormatParams = formatParams.ToImmutableArray ( );
        }
    }
}