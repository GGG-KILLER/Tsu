using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using Tsu.Numerics;

namespace Tsu.Benchmarks
{
    [SimpleJob ( RuntimeMoniker.Net48 )]
    [SimpleJob ( RuntimeMoniker.NetCoreApp31, baseline: true )]
    [SimpleJob ( RuntimeMoniker.NetCoreApp50 )]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser ( exportHtml: true, exportCombinedDisassemblyReport: true, exportDiff: true )]
    public class FileSizeFormatBranchingBenchmarks
    {
        private static readonly String[] _suffixes = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };

        [Params ( 512, FileSize.MiB, FileSize.MiB * 250.5 )]
        public Double Value { get; set; }

        public static (Double size, String suffix) Pre_GetFormatPair ( Double size )
        {
            if ( Double.IsInfinity ( size ) || Double.IsNaN ( size ) || size == 0D || size == -0D )
                return (size, "B");

            var power = ( Int32 ) Math.Max ( Math.Min ( Math.Floor ( Math.Log ( Math.Abs ( size ), 1024 ) ), 7 ), 0 );
            if ( power == 0 )
                return (size, "");
            return (size / Math.Pow ( 1024, power ), _suffixes[power]);
        }

        public static (Double size, String suffix) Post_GetFormatPair ( Double size )
        {
            if ( Double.IsInfinity ( size ) || Double.IsNaN ( size ) || size == 0D || size == -0D )
                return (size, "B");

            var abs = Math.Abs ( size );
            if ( abs >= FileSize.EiB )
                return (size / FileSize.EiB, nameof ( FileSize.EiB ));
            else if ( abs >= FileSize.PiB )
                return (size / FileSize.PiB, nameof ( FileSize.PiB ));
            else if ( abs >= FileSize.TiB )
                return (size / FileSize.TiB, nameof ( FileSize.TiB ));
            else if ( abs >= FileSize.GiB )
                return (size / FileSize.GiB, nameof ( FileSize.GiB ));
            else if ( abs >= FileSize.MiB )
                return (size / FileSize.MiB, nameof ( FileSize.MiB ));
            else if ( abs >= FileSize.KiB )
                return (size / FileSize.KiB, nameof ( FileSize.KiB ));
            else
                return (size, "B");
        }

        [Benchmark ( Baseline = true )]
        public (Double, String) OldGetFormatPair ( ) =>
            Pre_GetFormatPair ( this.Value );

        [Benchmark]
        public (Double, String) NewGetFormatPair ( ) =>
            Post_GetFormatPair ( this.Value );
    }
}