using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;

namespace GUtils.Benchmarks
{
    public class MyBenchmarkConfig : ManualConfig
    {
        public MyBenchmarkConfig ( )
        {
            Job @base = Job.Default.WithEvaluateOverhead ( true );

            Job[] jobs = new[]
            {
                @base.WithRuntime ( ClrRuntime.Net48 ),
                @base.WithRuntime ( CoreRuntime.Core31 ),
                @base.WithRuntime ( CoreRuntime.Core50 )
            };

            //this.AddJob ( Array.ConvertAll ( jobs, job => job.WithPlatform ( Platform.X86 ).WithGcServer ( false ) ) );
            //this.AddJob ( Array.ConvertAll ( jobs, job => job.WithPlatform ( Platform.X64 ).WithGcServer ( false ) ) );
            //this.AddJob ( Array.ConvertAll ( jobs, job => job.WithPlatform ( Platform.X86 ).WithGcServer ( true ) ) );
            this.AddJob ( Array.ConvertAll ( jobs, job => job.WithPlatform ( Platform.X64 ).WithGcServer ( true ) ) );
        }
    }
}