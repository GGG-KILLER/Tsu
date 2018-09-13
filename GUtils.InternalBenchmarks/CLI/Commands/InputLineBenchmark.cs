/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the “Software”), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
 * the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
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
