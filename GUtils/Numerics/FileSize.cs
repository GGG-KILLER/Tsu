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
using System.Globalization;
using System.Text.RegularExpressions;

namespace GUtils.Numerics
{
    /// <summary>
    /// An utility to deal with file sizes
    /// </summary>
    public static class FileSize
    {
        /// <summary>
        /// The regular expression used for parsing floating point file sizes. Accepts 0.0, 0.0B and 0.0KiB
        /// </summary>
        private static readonly Regex _floatParseRegex = new Regex ( @"^\s*(?<number>-?(?:\d+\.\d+|\d+|\.\d+))\s*(?<suffix>|B|(?:K|M|G|T|P|E)iB)\s*$",
                                                                     RegexOptions.Compiled | RegexOptions.CultureInvariant,
                                                                     TimeSpan.FromMilliseconds ( 250 ) );

        /// <summary>
        /// The regular expression used for parsing integer file sizes. Accepts 0, 0B and 0KiB
        /// </summary>
        private static readonly Regex _integerParseRegex = new Regex ( @"^\s*(?<number>-?\d+)\s*(?<suffix>|B|(?:K|M|G|T|P|E)iB)\s*$",
                                                                       RegexOptions.Compiled | RegexOptions.CultureInvariant,
                                                                       TimeSpan.FromMilliseconds ( 250 ) );

        /// <summary>
        /// A KiB (1024 B) in bytes.
        /// </summary>
        public const Int32 KiB = 1 << 10;

        /// <summary>
        /// A MiB (1024 KiB) in bytes.
        /// </summary>
        public const Int32 MiB = 1 << 20;

        /// <summary>
        /// A GiB (1024 MiB) in bytes.
        /// </summary>
        public const Int32 GiB = 1 << 30;

        /// <summary>
        /// A TiB (1024 GiB) in bytes.
        /// </summary>
        public const Int64 TiB = 1L << 40;

        /// <summary>
        /// A PiB (1024 TiB) in bytes.
        /// </summary>
        public const Int64 PiB = 1L << 50;

        /// <summary>
        /// An EiB (1024 PiB) in bytes.
        /// </summary>
        public const Int64 EiB = 1L << 60;

        /// <summary>
        /// Receives a file size (in bytes) and returns a tuple containing the scaled file size and
        /// the suffix to be used on display.
        /// </summary>
        /// <param name="size">The size (in bytes) to be formatted.</param>
        /// <returns>A tuple containing the scaled size and the suffix.</returns>
        public static (Double size, String suffix) GetFormatPair ( Int64 size )
        {
            var abs = Math.Abs ( size );
            if ( abs == 0 )
                return (size, "B");

            if ( abs >= EiB )
                return (size / ( Double ) EiB, nameof ( EiB ));
            else if ( abs >= PiB )
                return (size / ( Double ) PiB, nameof ( PiB ));
            else if ( abs >= TiB )
                return (size / ( Double ) TiB, nameof ( TiB ));
            else if ( abs >= GiB )
                return (size / ( Double ) GiB, nameof ( GiB ));
            else if ( abs >= MiB )
                return (size / ( Double ) MiB, nameof ( MiB ));
            else if ( abs >= KiB )
                return (size / ( Double ) KiB, nameof ( KiB ));
            else
                return (size, "B");
        }

        /// <summary>
        /// <inheritdoc cref="GetFormatPair(Int64)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="GetFormatPair(Int64)" /></param>
        /// <returns><inheritdoc cref="GetFormatPair(Int64)" /></returns>
        public static (Double size, String suffix) GetFormatPair ( Double size )
        {
            if ( Double.IsInfinity ( size ) || Double.IsNaN ( size ) || size == 0D || size == -0D )
                return (size, "B");

            var abs = Math.Abs ( size );
            if ( abs >= EiB )
                return (size / EiB, nameof ( EiB ));
            else if ( abs >= PiB )
                return (size / PiB, nameof ( PiB ));
            else if ( abs >= TiB )
                return (size / TiB, nameof ( TiB ));
            else if ( abs >= GiB )
                return (size / GiB, nameof ( GiB ));
            else if ( abs >= MiB )
                return (size / MiB, nameof ( MiB ));
            else if ( abs >= KiB )
                return (size / KiB, nameof ( KiB ));
            else
                return (size, "B");
        }

        /// <summary>
        /// Formats the provided file size in a human readable format (in bibytes).
        /// </summary>
        /// <param name="size"><inheritdoc cref="GetFormatPair(Int64)" /></param>
        /// <returns>A formatted string containing the scaled file size and the size suffix.</returns>
        public static String Format ( Int64 size )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return $"{newSize:0.##} {suffix}";
        }

        /// <summary>
        /// <inheritdoc cref="Format(Int64)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(Int64)" /></param>
        /// <returns><inheritdoc cref="Format(Int64)" /></returns>
        public static String Format ( Double size )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return $"{newSize:0.##} {suffix}";
        }

        /// <summary>
        /// <inheritdoc cref="Format(Int64)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(Int64)" /></param>
        /// <param name="format">
        /// The format template. Must contain two format placeholders. Default value is: <c>{0:0.##} {1}</c>
        /// </param>
        /// <returns><inheritdoc cref="Format(Int64)" /></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "There's already another overload for this." )]
        public static String Format ( Int64 size, String format )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return String.Format ( format, newSize, suffix );
        }

        /// <summary>
        /// <inheritdoc cref="Format(Double)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(Double)" /></param>
        /// <param name="format"><inheritdoc cref="Format(Int64, String)" /></param>
        /// <returns><inheritdoc cref="Format(Double)" /></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "There's already another overload for this." )]
        public static String Format ( Double size, String format )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return String.Format ( format, newSize, suffix );
        }

        /// <summary>
        /// <inheritdoc cref="Format(Int64, String)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(Int64, String)" /></param>
        /// <param name="format"><inheritdoc cref="Format(Int64, String)" /></param>
        /// <param name="provider">
        /// <inheritdoc cref="String.Format(IFormatProvider, String, Object, Object)" />
        /// </param>
        /// <returns><inheritdoc cref="Format(Int64, String)" /></returns>
        public static String Format ( Int64 size, String format, IFormatProvider provider )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return String.Format ( provider, format, newSize, suffix );
        }

        /// <summary>
        /// <inheritdoc cref="Format(Double, String)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(Double, String)" /></param>
        /// <param name="format"><inheritdoc cref="Format(Double, String)" /></param>
        /// <param name="provider">
        /// <inheritdoc cref="String.Format(IFormatProvider, String, Object, Object)" />
        /// </param>
        /// <returns><inheritdoc cref="Format(Double, String)" /></returns>
        public static String Format ( Double size, String format, IFormatProvider provider )
        {
            (var newSize, var suffix) = GetFormatPair ( size );
            return String.Format ( provider, format, newSize, suffix );
        }

        /// <summary>
        /// <inheritdoc cref="TryParseDouble(String, out Double)" />
        /// </summary>
        /// <param name="input"><inheritdoc cref="TryParseDouble(String, out Double)" /></param>
        /// <returns>
        /// <inheritdoc cref="TryParseDouble(String, out Double)" path="/param[@name='bytes']" />
        /// </returns>
        public static Double ParseDouble ( String input )
        {
            if ( TryParseInteger ( input, out var bytes ) )
                return bytes;
            throw new FormatException ( "The input string was in an unknown format." );
        }

        /// <summary>
        /// <inheritdoc cref="TryParseInteger(String, out Int64)" />
        /// </summary>
        /// <param name="input"><inheritdoc cref="TryParseInteger(String, out Int64)" /></param>
        /// <returns>
        /// <inheritdoc cref="TryParseInteger(String, out Int64)" path="/param[@name='bytes']" />
        /// </returns>
        public static Int64 ParseInteger ( String input )
        {
            if ( TryParseInteger ( input, out var bytes ) )
                return bytes;
            throw new FormatException ( "The input string was in an unknown format." );
        }

        /// <summary>
        /// Parses a file size in the format <c>(0|.0|0.0) (|B|KiB|MiB|GiB|TiB|PiB|EiB)</c>. Might
        /// suffer from precision loss.
        /// </summary>
        /// <param name="input">The input string to be parsed.</param>
        /// <param name="bytes">The amount of bytes that the inputted string is equivalent to.</param>
        /// <returns>Whether the amount of bytes was parsed successfully</returns>
        public static Boolean TryParseDouble ( String input, out Double bytes )
        {
            Match match = _floatParseRegex.Match ( input );
            if ( !match.Success )
            {
                bytes = default;
                return false;
            }

            if ( !Double.TryParse ( match.Groups["number"].Value, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var number ) )
            {
                bytes = default;
                return false;
            }

            switch ( match.Groups["suffix"].Value )
            {
                case "":
                case "B":
                    bytes = number;
                    return true;

                case "KiB":
                    bytes = number * KiB;
                    return true;

                case "MiB":
                    bytes = number * MiB;
                    return true;

                case "GiB":
                    bytes = number * GiB;
                    return true;

                case "TiB":
                    bytes = number * TiB;
                    return true;

                case "PiB":
                    bytes = number * PiB;
                    return true;

                case "EiB":
                    bytes = number * EiB;
                    return true;

                default:
                    bytes = default;
                    return false;
            }
        }

        /// <summary>
        /// Parses a file size in the format <c>0 (|B|KiB|MiB|GiB|TiB|PiB|EiB)</c>. Might suffer
        /// from precision loss although less likely than with the <see cref="Double" />-accepting counterpart.
        /// </summary>
        /// <param name="input">The input string to be parsed.</param>
        /// <param name="bytes">The amount of bytes that the inputted string is equivalent to.</param>
        /// <returns>Whether the amount of bytes was parsed successfully.</returns>
        public static Boolean TryParseInteger ( String input, out Int64 bytes )
        {
            Match match = _integerParseRegex.Match ( input );
            if ( !match.Success )
            {
                bytes = default;
                return false;
            }

            if ( !Int64.TryParse ( match.Groups["number"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number ) )
            {
                bytes = default;
                return false;
            }

            switch ( match.Groups["suffix"].Value )
            {
                case "":
                case "B":
                    bytes = number;
                    return true;

                case "KiB":
                    bytes = number * KiB;
                    return true;

                case "MiB":
                    bytes = number * MiB;
                    return true;

                case "GiB":
                    bytes = number * GiB;
                    return true;

                case "TiB":
                    bytes = number * TiB;
                    return true;

                case "PiB":
                    bytes = number * PiB;
                    return true;

                case "EiB":
                    bytes = number * EiB;
                    return true;

                default:
                    bytes = default;
                    return false;
            }
        }
    }
}