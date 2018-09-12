using System;
using System.Text;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace GUtils.InternalBenchmarks
{
    class Program
    {
        static void Main ( String[] args )
        {
            BenchmarkSwitcher.FromAssembly ( typeof ( Program ).Assembly )
                .Run ( args, DefaultConfig.Instance.With ( Encoding.UTF8 ).KeepBenchmarkFiles ( false ) );
        }
    }
}
