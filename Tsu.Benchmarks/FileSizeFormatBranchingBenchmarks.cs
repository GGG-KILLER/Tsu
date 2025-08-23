// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Tsu.Numerics;

namespace Tsu.Benchmarks
{
#if !NET8_0_OR_GREATER
    [SimpleJob(RuntimeMoniker.Net48)]
#endif
    [SimpleJob(RuntimeMoniker.Net80, baseline: true)]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(exportHtml: true, exportCombinedDisassemblyReport: true, exportDiff: true)]
    public class FileSizeFormatBranchingBenchmarks
    {
        private static readonly string[] _suffixes = ["B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB"];

        [Params(512, FileSize.MiB, FileSize.MiB * 250.5)]
        public double Value { get; set; }

        public static (double size, string suffix) Pre_GetFormatPair(double size)
        {
            if (double.IsInfinity(size) || double.IsNaN(size) || size == 0D || size == -0D)
                return (size, "B");

            var power = (int) Math.Max(Math.Min(Math.Floor(Math.Log(Math.Abs(size), 1024)), 7), 0);
            if (power == 0)
                return (size, "");
            return (size / Math.Pow(1024, power), _suffixes[power]);
        }

        public static (double size, string suffix) Post_GetFormatPair(double size)
        {
            if (double.IsInfinity(size) || double.IsNaN(size) || size == 0D || size == -0D)
                return (size, "B");

            var abs = Math.Abs(size);
            if (abs >= FileSize.EiB)
                return (size / FileSize.EiB, nameof(FileSize.EiB));
            else if (abs >= FileSize.PiB)
                return (size / FileSize.PiB, nameof(FileSize.PiB));
            else if (abs >= FileSize.TiB)
                return (size / FileSize.TiB, nameof(FileSize.TiB));
            else if (abs >= FileSize.GiB)
                return (size / FileSize.GiB, nameof(FileSize.GiB));
            else if (abs >= FileSize.MiB)
                return (size / FileSize.MiB, nameof(FileSize.MiB));
            else if (abs >= FileSize.KiB)
                return (size / FileSize.KiB, nameof(FileSize.KiB));
            else
                return (size, "B");
        }

        [Benchmark(Baseline = true)]
        public (double, string) OldGetFormatPair() =>
            Pre_GetFormatPair(Value);

        [Benchmark]
        public (double, string) NewGetFormatPair() =>
            Post_GetFormatPair(Value);
    }
}
