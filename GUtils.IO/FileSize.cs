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

namespace GUtils.IO
{
    /// <summary>
    /// An utility to deal with file sizes
    /// </summary>
    public static class FileSize
    {
        /// <summary>
        /// A KiB in bytes
        /// </summary>
        public const Int32 KiB = 1 << 10;

        /// <summary>
        /// A MiB in bytes
        /// </summary>
        public const Int32 MiB = 1 << 20;

        /// <summary>
        /// A GiB in bytes
        /// </summary>
        public const Int32 GiB = 1 << 30;

        /// <summary>
        /// A TiB in bytes
        /// </summary>
        public const Int64 TiB = 1 << 40;

        /// <summary>
        /// A PiB in bytes
        /// </summary>
        public const Int64 PiB = 1 << 50;

        /// <summary>
        /// An EiB in bytes
        /// </summary>
        public const Int64 EiB = 1 << 60;

        private static readonly String[] _suffixes = new[] { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };

        /// <summary>
        /// Returns the size provided as a pair of the new size and the suffix
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static (Double, String) GetFormatPair ( Int64 size )
        {
            if ( size == 0 ) return (size, "B");
            var i = ( Int32 ) Math.Floor ( Math.Log ( Math.Abs ( size ), 1024 ) );
            return (size / Math.Pow ( 1024, i ), _suffixes[i]);
        }

        /// <summary>
        /// Returns the size provided as a pair of the new size and the suffix
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static (Double, String) GetFormatPair ( Double size )
        {
            if ( Double.IsInfinity ( size ) || Double.IsNaN ( size ) || size == 0D || size == -0D )
                return (size, "B");
            var i = ( Int32 ) Math.Floor ( Math.Log ( Math.Abs ( size ), 1024 ) );
            return (size / Math.Pow ( 1024, i ), _suffixes[i]);
        }

        /// <summary>
        /// Formats the provided file size in a human readable format (in bibytes)
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static String Format ( Int64 size )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return $"{newSize:0.##} {suffix}";
        }

        /// <summary>
        /// Formats the provided file size in a human readable format (in bibytes)
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static String Format ( Double size )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return $"{newSize:0.##} {suffix}";
        }

        /// <summary>
        /// Formats the provided file size in a human readable format (in bibytes)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="format">default value is: "{0} {1}"</param>
        /// <returns></returns>
        public static String Format ( Int64 size, String format )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return String.Format ( format, newSize, suffix );
        }

        /// <summary>
        /// Formats the provided file size in a human readable format (in bibytes)
        /// </summary>
        /// <param name="size"></param>
        /// <param name="format">default value is: "{0} {1}"</param>
        /// <returns></returns>
        public static String Format ( Double size, String format )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return String.Format ( format, newSize, suffix );
        }
    }
}
