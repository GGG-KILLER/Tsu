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

namespace GUtils.Timing
{
    public static class Duration
    {
        public const Double TicksPerHour = TimeSpan.TicksPerHour;
        public const Double TicksPerMinute = TicksPerHour / 60D;
        public const Double TicksPerSecond = TicksPerMinute / 60D;
        public const Double TicksPerMillisecond = TicksPerSecond / 1000D;
        public const Double TicksPerMicrosecond = TicksPerMillisecond / 1000D;
        public const Double TicksPerNanosecond = TicksPerMicrosecond / 1000D;

        public static String Format ( Int64 ticks, String format = "{0:##00.00}{1}" )
        {
            if ( ticks > TicksPerHour )
                return String.Format ( format, ticks / TicksPerHour, "h" );
            else if ( ticks > TicksPerMinute )
                return String.Format ( format, ticks / TicksPerMinute, "m" );
            else if ( ticks > TicksPerSecond )
                return String.Format ( format, ticks / TicksPerSecond, "s" );
            else if ( ticks > TicksPerMillisecond )
                return String.Format ( format, ticks / TicksPerMillisecond, "ms" );
            else if ( ticks > TicksPerMicrosecond )
                return String.Format ( format, ticks / TicksPerMicrosecond, "μs" );
            else
                return String.Format ( format, ticks / TicksPerNanosecond, "ns" );
        }
    }
}
