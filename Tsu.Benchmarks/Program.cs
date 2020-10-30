using System;
using BenchmarkDotNet.Running;

namespace Tsu.Benchmarks
{
    internal class Program
    {
        private static void Main ( String[] args ) =>
            BenchmarkSwitcher.FromAssembly ( typeof ( Program ).Assembly ).Run ( args );
    }
}