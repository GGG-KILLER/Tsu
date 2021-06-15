// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tsu.Numerics
{
    /// <summary>
    /// An utility to deal with file sizes
    /// </summary>
    public static class FileSize
    {
        /// <summary>
        /// The regular expression used for parsing floating point file sizes. Accepts 0.0, 0.0B and 0.0KiB
        /// </summary>
        private static readonly Regex _floatParseRegex = new Regex(@"^\s*(?<number>-?(?:\d+\.\d+|\d+|\.\d+))\s*(?<suffix>|B|(?:K|M|G|T|P|E)iB)\s*$",
                                                                   RegexOptions.Compiled | RegexOptions.CultureInvariant,
                                                                   TimeSpan.FromMilliseconds(250));

        /// <summary>
        /// The regular expression used for parsing integer file sizes. Accepts 0, 0B and 0KiB
        /// </summary>
        private static readonly Regex _integerParseRegex = new Regex(@"^\s*(?<number>-?\d+)\s*(?<suffix>|B|(?:K|M|G|T|P|E)iB)\s*$",
                                                                     RegexOptions.Compiled | RegexOptions.CultureInvariant,
                                                                     TimeSpan.FromMilliseconds(250));

        /// <summary>
        /// A KiB (1024 B) in bytes.
        /// </summary>
        public const int KiB = 1 << 10;

        /// <summary>
        /// A MiB (1024 KiB) in bytes.
        /// </summary>
        public const int MiB = 1 << 20;

        /// <summary>
        /// A GiB (1024 MiB) in bytes.
        /// </summary>
        public const int GiB = 1 << 30;

        /// <summary>
        /// A TiB (1024 GiB) in bytes.
        /// </summary>
        public const long TiB = 1L << 40;

        /// <summary>
        /// A PiB (1024 TiB) in bytes.
        /// </summary>
        public const long PiB = 1L << 50;

        /// <summary>
        /// An EiB (1024 PiB) in bytes.
        /// </summary>
        public const long EiB = 1L << 60;

        /// <summary>
        /// Receives a file size (in bytes) and returns a tuple containing the scaled file size and
        /// the suffix to be used on display.
        /// </summary>
        /// <param name="size">The size (in bytes) to be formatted.</param>
        /// <param name="scaledSize">The scaled down size.</param>
        /// <param name="suffix">The suffix.</param>
        public static void GetFormatPair(long size, out double scaledSize, out string suffix)
        {
            var abs = Math.Abs(size);
            if (abs == 0)
            {
                scaledSize = size;
                suffix = "B";
                return;
            }

            if (abs >= EiB)
            {
                scaledSize = size / (double) EiB;
                suffix = nameof(EiB);
                return;
            }
            else if (abs >= PiB)
            {
                scaledSize = size / (double) PiB;
                suffix = nameof(PiB);
                return;
            }
            else if (abs >= TiB)
            {
                scaledSize = size / (double) TiB;
                suffix = nameof(TiB);
                return;
            }
            else if (abs >= GiB)
            {
                scaledSize = size / (double) GiB;
                suffix = nameof(GiB);
                return;
            }
            else if (abs >= MiB)
            {
                scaledSize = size / (double) MiB;
                suffix = nameof(MiB);
                return;
            }
            else if (abs >= KiB)
            {
                scaledSize = size / (double) KiB;
                suffix = nameof(KiB);
                return;
            }
            else
            {
                scaledSize = size;
                suffix = "B";
                return;
            }
        }

        /// <summary>
        /// <inheritdoc cref="GetFormatPair(long, out double, out string)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="GetFormatPair(long, out double, out string)" /></param>
        /// <param name="scaledSize"><inheritdoc cref="GetFormatPair(long, out double, out string)" /></param>
        /// <param name="suffix"><inheritdoc cref="GetFormatPair(long, out double, out string)" /></param>
        public static void GetFormatPair(double size, out double scaledSize, out string suffix)
        {
            if (double.IsInfinity(size) || double.IsNaN(size) || size == 0D || size == -0D)
            {
                scaledSize = size;
                suffix = "B";
                return;
            }

            var abs = Math.Abs(size);
            if (abs >= EiB)
            {
                scaledSize = size / EiB;
                suffix = nameof(EiB);
                return;
            }
            else if (abs >= PiB)
            {
                scaledSize = size / PiB;
                suffix = nameof(PiB);
                return;
            }
            else if (abs >= TiB)
            {
                scaledSize = size / TiB;
                suffix = nameof(TiB);
                return;
            }
            else if (abs >= GiB)
            {
                scaledSize = size / GiB;
                suffix = nameof(GiB);
                return;
            }
            else if (abs >= MiB)
            {
                scaledSize = size / MiB;
                suffix = nameof(MiB);
                return;
            }
            else if (abs >= KiB)
            {
                scaledSize = size / KiB;
                suffix = nameof(KiB);
                return;
            }
            else
            {
                scaledSize = size;
                suffix = "B";
                return;
            }
        }

        /// <summary>
        /// Formats the provided file size in a human readable format (in bibytes).
        /// </summary>
        /// <param name="size"><inheritdoc cref="GetFormatPair(long, out double, out string)" /></param>
        /// <returns>A formatted string containing the scaled file size and the size suffix.</returns>
        public static string Format(long size)
        {
            GetFormatPair(size, out var newSize, out var suffix);
            return $"{newSize:0.##} {suffix}";
        }

        /// <summary>
        /// <inheritdoc cref="Format(long)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(long)" /></param>
        /// <returns><inheritdoc cref="Format(long)" /></returns>
        public static string Format(double size)
        {
            GetFormatPair(size, out var newSize, out var suffix);
            return $"{newSize:0.##} {suffix}";
        }

        /// <summary>
        /// <inheritdoc cref="Format(long)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(long)" /></param>
        /// <param name="format">
        /// The format template. Must contain two format placeholders. Default value is: <c>{0:0.##} {1}</c>
        /// </param>
        /// <returns><inheritdoc cref="Format(long)" /></returns>
        [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "There's already another overload for this.")]
        public static string Format(long size, string format)
        {
            GetFormatPair(size, out var newSize, out var suffix);
            return string.Format(format, newSize, suffix);
        }

        /// <summary>
        /// <inheritdoc cref="Format(double)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(double)" /></param>
        /// <param name="format"><inheritdoc cref="Format(long, string)" /></param>
        /// <returns><inheritdoc cref="Format(double)" /></returns>
        [SuppressMessage("Globalization", "CA1305:Specify IFormatProvider", Justification = "There's already another overload for this.")]
        public static string Format(double size, string format)
        {
            GetFormatPair(size, out var newSize, out var suffix);
            return string.Format(format, newSize, suffix);
        }

        /// <summary>
        /// <inheritdoc cref="Format(long, string)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(long, string)" /></param>
        /// <param name="format"><inheritdoc cref="Format(long, string)" /></param>
        /// <param name="provider">
        /// <inheritdoc cref="string.Format(IFormatProvider, string, object, object)" />
        /// </param>
        /// <returns><inheritdoc cref="Format(long, string)" /></returns>
        public static string Format(long size, string format, IFormatProvider provider)
        {
            GetFormatPair(size, out var newSize, out var suffix);
            return string.Format(provider, format, newSize, suffix);
        }

        /// <summary>
        /// <inheritdoc cref="Format(double, string)" />
        /// </summary>
        /// <param name="size"><inheritdoc cref="Format(double, string)" /></param>
        /// <param name="format"><inheritdoc cref="Format(double, string)" /></param>
        /// <param name="provider">
        /// <inheritdoc cref="string.Format(IFormatProvider, string, object, object)" />
        /// </param>
        /// <returns><inheritdoc cref="Format(double, string)" /></returns>
        public static string Format(double size, string format, IFormatProvider provider)
        {
            GetFormatPair(size, out var newSize, out var suffix);
            return string.Format(provider, format, newSize, suffix);
        }

        /// <summary>
        /// <inheritdoc cref="TryParseDouble(string, out double)" />
        /// </summary>
        /// <param name="input"><inheritdoc cref="TryParseDouble(string, out double)" /></param>
        /// <returns>
        /// <inheritdoc cref="TryParseDouble(string, out double)" path="/param[@name='bytes']" />
        /// </returns>
        public static double ParseDouble(string input)
        {
            if (TryParseInteger(input, out var bytes))
                return bytes;
            throw new FormatException("The input string was in an unknown format.");
        }

        /// <summary>
        /// <inheritdoc cref="TryParseInteger(string, out long)" />
        /// </summary>
        /// <param name="input"><inheritdoc cref="TryParseInteger(string, out long)" /></param>
        /// <returns>
        /// <inheritdoc cref="TryParseInteger(string, out long)" path="/param[@name='bytes']" />
        /// </returns>
        public static long ParseInteger(string input)
        {
            if (TryParseInteger(input, out var bytes))
                return bytes;
            throw new FormatException("The input string was in an unknown format.");
        }

        /// <summary>
        /// Parses a file size in the format <c>(0|.0|0.0) (|B|KiB|MiB|GiB|TiB|PiB|EiB)</c>. Might
        /// suffer from precision loss.
        /// </summary>
        /// <param name="input">The input string to be parsed.</param>
        /// <param name="bytes">The amount of bytes that the inputted string is equivalent to.</param>
        /// <returns>Whether the amount of bytes was parsed successfully</returns>
        public static bool TryParseDouble(string input, out double bytes)
        {
            var match = _floatParseRegex.Match(input);
            if (!match.Success)
            {
                bytes = default;
                return false;
            }

            if (!double.TryParse(match.Groups["number"].Value, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var number))
            {
                bytes = default;
                return false;
            }

            switch (match.Groups["suffix"].Value)
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
        /// from precision loss although less likely than with the <see cref="double" />-accepting counterpart.
        /// </summary>
        /// <param name="input">The input string to be parsed.</param>
        /// <param name="bytes">The amount of bytes that the inputted string is equivalent to.</param>
        /// <returns>Whether the amount of bytes was parsed successfully.</returns>
        public static bool TryParseInteger(string input, out long bytes)
        {
            var match = _integerParseRegex.Match(input);
            if (!match.Success)
            {
                bytes = default;
                return false;
            }

            if (!long.TryParse(match.Groups["number"].Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
            {
                bytes = default;
                return false;
            }

            switch (match.Groups["suffix"].Value)
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