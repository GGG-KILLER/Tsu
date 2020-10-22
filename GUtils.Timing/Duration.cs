/*
 * Copyright © 2019 GGG KILLER <gggkiller2@gmail.com>
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
using System.Diagnostics.CodeAnalysis;

namespace GUtils.Timing
{
    /// <summary>
    /// An utility class to help manipulate time as ticks
    /// </summary>
    public static class Duration
    {
        /// <summary>
        /// The amount of tikcs in an hour
        /// </summary>
        public const Double TicksPerHour = TimeSpan.TicksPerHour;

        /// <summary>
        /// The amount of ticks in a minute
        /// </summary>
        public const Double TicksPerMinute = TicksPerHour / 60D;

        /// <summary>
        /// The amount of ticks in a second
        /// </summary>
        public const Double TicksPerSecond = TicksPerMinute / 60D;

        /// <summary>
        /// The amount of ticks in a millisecond
        /// </summary>
        public const Double TicksPerMillisecond = TicksPerSecond / 1000D;

        /// <summary>
        /// The amount of ticks in a microsecond
        /// </summary>
        public const Double TicksPerMicrosecond = TicksPerMillisecond / 1000D;

        /// <summary>
        /// The amount of ticks in a nanosecond
        /// </summary>
        public const Double TicksPerNanosecond = TicksPerMicrosecond / 1000D;

        /// <summary>
        /// Formats the amount of ticks provided into a human
        /// readable format.
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "There's another overload accepting it." )]
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

        /// <summary>
        /// Formats the amount of ticks provided into a human
        /// readable format.
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static String Format ( Int64 ticks, IFormatProvider formatProvider, String format = "{0:##00.00}{1}" )
        {
            if ( ticks > TicksPerHour )
                return String.Format ( formatProvider, format, ticks / TicksPerHour, "h" );
            else if ( ticks > TicksPerMinute )
                return String.Format ( formatProvider, format, ticks / TicksPerMinute, "m" );
            else if ( ticks > TicksPerSecond )
                return String.Format ( formatProvider, format, ticks / TicksPerSecond, "s" );
            else if ( ticks > TicksPerMillisecond )
                return String.Format ( formatProvider, format, ticks / TicksPerMillisecond, "ms" );
            else if ( ticks > TicksPerMicrosecond )
                return String.Format ( formatProvider, format, ticks / TicksPerMicrosecond, "μs" );
            else
                return String.Format ( formatProvider, format, ticks / TicksPerNanosecond, "ns" );
        }
    }
}
