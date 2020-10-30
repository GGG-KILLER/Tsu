using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace Tsu.Benchmarks
{
    [SimpleJob ( RuntimeMoniker.Net48 )]
    [SimpleJob ( RuntimeMoniker.NetCoreApp31, baseline: true )]
    [SimpleJob ( RuntimeMoniker.NetCoreApp50 )]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser ( exportHtml: true, exportCombinedDisassemblyReport: true, exportDiff: true )]
    public class NumberScaleMicrobenchmark
    {
        private const Double inv3 = 1d / 3d;
        private const Double log1000 = 6.90775527898214;
        private const Double invLog1000 = 1 / log1000;

        [Params ( 10 )]
        public Double Value { get; set; }

        [Benchmark ( Baseline = true )]
        public Double FloorLogWithBase ( ) =>
            Math.Floor ( Math.Log ( this.Value, 1000 ) );

        [Benchmark ( )]
        public Double CastLogWithBase ( ) =>
            ( Int32 ) Math.Log ( this.Value, 1000 );

        [Benchmark]
        public Double FloorLogWithDivision ( ) =>
            Math.Floor ( Math.Log ( this.Value ) / log1000 );

        [Benchmark]
        public Double CastLogWithDivision ( ) =>
            ( Int32 ) ( Math.Log ( this.Value ) / log1000 );

        [Benchmark]
        public Double Log10WithDivision ( ) =>
            Math.Floor ( Math.Log10 ( this.Value ) / 3d );

        [Benchmark]
        public Double CastLogWithMultiplication ( ) =>
            ( Int32 ) ( Math.Log ( this.Value ) * invLog1000 );

        [Benchmark]
        public Double FloorLogWithMultiplication ( ) =>
            Math.Floor ( Math.Log ( this.Value ) * invLog1000 );

        [Benchmark]
        public Double CastLog10WithMultiplication ( ) =>
            ( Int32 ) ( Math.Log10 ( this.Value ) * inv3 );

        [Benchmark]
        public Double FloorLog10WithMultiplication ( ) =>
            Math.Floor ( Math.Log10 ( this.Value ) * inv3 );

        [Benchmark]
        public Int32 BranchingScaling ( )
        {
            if ( this.Value >= 1_000_000_000_000_000_000 )
                return 6;
            else if ( this.Value >= 1_000_000_000_000_000 )
                return 5;
            else if ( this.Value >= 1_000_000_000_000 )
                return 4;
            else if ( this.Value >= 1_000_000_000 )
                return 3;
            else if ( this.Value >= 1_000_000 )
                return 2;
            else if ( this.Value >= 1_000 )
                return 1;
            else
                return 0;
        }
    }
}