// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
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
using System.Diagnostics.CodeAnalysis;

namespace Tsu.Numerics
{
    /// <summary>
    /// An utility class to help manipulate time as ticks
    /// </summary>
#if IS_MICROPROFILER_PACKAGE
    internal
#else
    public
#endif
        static class Duration
    {
        /// <summary>
        /// The amount of tikcs in an hour
        /// </summary>
        public const double TicksPerHour = TimeSpan.TicksPerHour;

        /// <summary>
        /// The amount of ticks in a minute
        /// </summary>
        public const double TicksPerMinute = TicksPerHour / 60D;

        /// <summary>
        /// The amount of ticks in a second
        /// </summary>
        public const double TicksPerSecond = TicksPerMinute / 60D;

        /// <summary>
        /// The amount of ticks in a millisecond
        /// </summary>
        public const double TicksPerMillisecond = TicksPerSecond / 1000D;

        /// <summary>
        /// The amount of ticks in a microsecond
        /// </summary>
        public const double TicksPerMicrosecond = TicksPerMillisecond / 1000D;

        /// <summary>
        /// The amount of ticks in a nanosecond
        /// </summary>
        public const double TicksPerNanosecond = TicksPerMicrosecond / 1000D;

        /// <summary>
        /// Scales the provided value down and gets the duration from the ticks.
        /// </summary>
        /// <param name="ticks">The tick count.</param>
        /// <param name="scaledDuration">The scaled down duration.</param>
        /// <param name="suffix">The suffix.</param>
        public static void GetFormatPair(long ticks, out double scaledDuration, out string suffix)
        {
            if (ticks > TicksPerHour)
            {
                scaledDuration = ticks / TicksPerHour;
                suffix = "h";
                return;
            }
            else if (ticks > TicksPerMinute)
            {
                scaledDuration = ticks / TicksPerMinute;
                suffix = "m";
                return;
            }
            else if (ticks > TicksPerSecond)
            {
                scaledDuration = ticks / TicksPerSecond;
                suffix = "s";
                return;
            }
            else if (ticks > TicksPerMillisecond)
            {
                scaledDuration = ticks / TicksPerMillisecond;
                suffix = "ms";
                return;
            }
            else if (ticks > TicksPerMicrosecond)
            {
                scaledDuration = ticks / TicksPerMicrosecond;
                suffix = "μs";
                return;
            }
            else
            {
                scaledDuration = ticks / TicksPerNanosecond;
                suffix = "ns";
                return;
            }
        }

        /// <summary>
        /// Formats the amount of ticks provided into a human
        /// readable format.
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "There's another overload accepting it.")]
        public static string Format(long ticks, string format = "{0:##00.00}{1}")
        {
            GetFormatPair(ticks, out var scaledDuration, out var suffix);
            return string.Format(format, scaledDuration, suffix);
        }

        /// <summary>
        /// Formats the amount of ticks provided into a human
        /// readable format.
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="formatProvider"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string Format(long ticks, IFormatProvider formatProvider, string format = "{0:##00.00}{1}")
        {
            GetFormatPair(ticks, out var scaledDuration, out var suffix);
            return string.Format(formatProvider, format, scaledDuration, suffix);
        }
    }
}
