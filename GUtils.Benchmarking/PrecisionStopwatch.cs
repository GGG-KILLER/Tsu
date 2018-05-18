using System;
using System.Diagnostics;

namespace GUtils.Benchmarking
{
    public class PrecisionStopwatch : Stopwatch
    {
        public const Double TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000D;

        public Double ElapsedMicroseconds => this.ElapsedTicks / TicksPerMicrosecond;
    }
}
