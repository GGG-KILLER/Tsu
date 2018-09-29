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

namespace GUtils.IO
{
    public static class FileSize
    {
        public const Int64 KiB = 1 << 10;
        public const Int64 MiB = 1 << 20;
        public const Int64 GiB = 1 << 30;
        public const Int64 TiB = 1 << 40;
        public const Int64 PiB = 1 << 50;
        public const Int64 EiB = 1 << 60;

        private static readonly String[] _suffixes = new[] { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };

        public static (Double, String) GetFormatPair ( Int64 size )
        {
            var i = ( Int32 ) Math.Floor ( Math.Log ( size, 1024 ) );
            return (size / Math.Pow ( 1024, i ), _suffixes[i]);
        }

        public static (Double, String) GetFormatPair ( Double size )
        {
            var i = ( Int32 ) Math.Floor ( Math.Log ( size, 1024 ) );
            return (size / Math.Pow ( 1024, i ), _suffixes[i]);
        }

        public static String Format ( Int64 size )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return $"{newSize:.##} {suffix}";
        }

        public static String Format ( Double size )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return $"{newSize:.##} {suffix}";
        }

        public static String Format ( Int64 size, String format )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return String.Format ( format, newSize, suffix );
        }

        public static String Format ( Double size, String format )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return String.Format ( format, newSize, suffix );
        }
    }
}
