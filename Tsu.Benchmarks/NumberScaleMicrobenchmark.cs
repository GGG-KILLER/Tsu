// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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

namespace Tsu.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net48)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31, baseline: true)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(exportHtml: true, exportCombinedDisassemblyReport: true, exportDiff: true)]
    public class NumberScaleMicrobenchmark
    {
        private const double inv3 = 1d / 3d;
        private const double log1000 = 6.90775527898214;
        private const double invLog1000 = 1 / log1000;

        [Params(10)]
        public double Value { get; set; }

        [Benchmark(Baseline = true)]
        public double FloorLogWithBase() =>
            Math.Floor(Math.Log(Value, 1000));

        [Benchmark()]
        public double CastLogWithBase() =>
            (int) Math.Log(Value, 1000);

        [Benchmark]
        public double FloorLogWithDivision() =>
            Math.Floor(Math.Log(Value) / log1000);

        [Benchmark]
        public double CastLogWithDivision() =>
            (int) (Math.Log(Value) / log1000);

        [Benchmark]
        public double Log10WithDivision() =>
            Math.Floor(Math.Log10(Value) / 3d);

        [Benchmark]
        public double CastLogWithMultiplication() =>
            (int) (Math.Log(Value) * invLog1000);

        [Benchmark]
        public double FloorLogWithMultiplication() =>
            Math.Floor(Math.Log(Value) * invLog1000);

        [Benchmark]
        public double CastLog10WithMultiplication() =>
            (int) (Math.Log10(Value) * inv3);

        [Benchmark]
        public double FloorLog10WithMultiplication() =>
            Math.Floor(Math.Log10(Value) * inv3);

        [Benchmark]
        public int BranchingScaling()
        {
            if (Value >= 1_000_000_000_000_000_000)
                return 6;
            else if (Value >= 1_000_000_000_000_000)
                return 5;
            else if (Value >= 1_000_000_000_000)
                return 4;
            else if (Value >= 1_000_000_000)
                return 3;
            else if (Value >= 1_000_000)
                return 2;
            else if (Value >= 1_000)
                return 1;
            else
                return 0;
        }
    }
}