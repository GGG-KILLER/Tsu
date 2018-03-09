using System;

namespace GUtils.Timing
{
    public static class Timespans
    {
        public const Double TicksPerHour = TimeSpan.TicksPerHour;
        public const Double TicksPerMinute = TicksPerHour / 60D;
        public const Double TicksPerSecond = TicksPerMinute / 60D;
        public const Double TicksPerMillisecond = TicksPerSecond / 1000D;
        public const Double TicksPerMicrosecond = TicksPerMillisecond / 1000D;
        public const Double TicksPerNanosecond = TicksPerMicrosecond / 1000D;

        public static String Format ( Int64 Ticks, String Format = "{0:##00.00}{1}" )
        {
            if ( Ticks > TicksPerHour )
                return String.Format ( Format, Ticks / TicksPerHour, "h" );
            else if ( Ticks > TicksPerMinute )
                return String.Format ( Format, Ticks / TicksPerMinute, "m" );
            else if ( Ticks > TicksPerSecond )
                return String.Format ( Format, Ticks / TicksPerSecond, "s" );
            else if ( Ticks > TicksPerMillisecond )
                return String.Format ( Format, Ticks / TicksPerMillisecond, "ms" );
            else if ( Ticks > TicksPerMicrosecond )
                return String.Format ( Format, Ticks / TicksPerMicrosecond, "μs" );
            else
                return String.Format ( Format, Ticks / TicksPerNanosecond, "ns" );
        }
    }
}
