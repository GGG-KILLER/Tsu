using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using GUtils.CLI.Commands;

namespace GUtils.InternalBenchmarks.CLI.Commands
{
    [ClrJob, CoreJob ( true ), DryCoreJob, DryClrJob]
    public class InputLineBenchmark
    {
        [Benchmark ( Description = "Simple parsing" )]
        [Arguments ( @"some long input ""string for the"" parser" )]
        public String[] SimpleParse ( String input )
        {
            return InputLineParser.SimpleParse ( input ).ToArray ( );
        }

        [Benchmark ( Description = "Complex parsing")]
        [Arguments ( @"let\'s ""\o163\b1110100\x61\o162\o164 \o142\o171\x20\o163\97y\105\b1101110\x67"" r:\o150\b1100101\o154\x6c\x6f\x20\b1110111\o157\x72\o154\x64 how are you?" )]
        public String[] ComplexParse ( String input )
        {
            return InputLineParser.Parse ( input ).ToArray ( );
        }
    }
}
