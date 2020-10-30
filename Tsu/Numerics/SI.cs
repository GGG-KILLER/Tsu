using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tsu.Numerics
{
    /// <summary>
    /// A subset of the International System of Units Prefixes that only contains the prefixes that
    /// are powers of 1000
    /// </summary>
    public static class SI
    {
        /// <summary>
        /// The regular expression used for parsing floating point file sizes. Accepts 0.0, 0.0B and 0.0KiB
        /// </summary>
        private static readonly Regex _floatParseRegex = new Regex ( @"^\s*(?<number>-?(?:\d+\.\d+|\d+|\.\d+))\s*(?<suffix>y|z|a|f|p|n|u|μ|m||k|M|G|T|P|E|Z|Y)\w*\s*$",
                                                                     RegexOptions.Compiled | RegexOptions.CultureInvariant,
                                                                     TimeSpan.FromMilliseconds ( 250 ) );

        /// <summary>
        /// A yotta (Y)
        /// </summary>
        public const Double Yotta = Zetta * 1000;

        /// <summary>
        /// A zetta (Z)
        /// </summary>
        public const Double Zetta = Exa * 1000;

        /// <summary>
        /// An exa (E)
        /// </summary>
        public const Double Exa = Peta * 1000;

        /// <summary>
        /// A peta (P)
        /// </summary>
        public const Double Peta = Tera * 1000;

        /// <summary>
        /// A tera (T)
        /// </summary>
        public const Double Tera = Giga * 1000;

        /// <summary>
        /// A giga (G)
        /// </summary>
        public const Double Giga = Mega * 1000;

        /// <summary>
        /// A mega (M)
        /// </summary>
        public const Double Mega = Kilo * 1000;

        /// <summary>
        /// A kilo (k)
        /// </summary>
        public const Double Kilo = 1000;

        /// <summary>
        /// A milli (m)
        /// </summary>
        public const Double Milli = 0.001;

        /// <summary>
        /// A micro (μ)
        /// </summary>
        public const Double Micro = Milli * 0.001;

        /// <summary>
        /// A nano (n)
        /// </summary>
        public const Double Nano = Micro * 0.001;

        /// <summary>
        /// A pico (p)
        /// </summary>
        public const Double Pico = Nano * 0.001;

        /// <summary>
        /// A femto (f)
        /// </summary>
        public const Double Femto = Pico * 0.001;

        /// <summary>
        /// An atto (a)
        /// </summary>
        public const Double Atto = Femto * 0.001;

        /// <summary>
        /// A zepto (z)
        /// </summary>
        public const Double Zepto = Atto * 0.001;

        /// <summary>
        /// A yocto (y)
        /// </summary>
        public const Double Yocto = Zepto * 0.001;

        /// <summary>
        /// Returns the pair of objects used in suffixing a SI number
        /// </summary>
        /// <param name="number">The number to scale down and get the suffix of.</param>
        /// <param name="scaledNumber">The scaled down number.</param>
        /// <param name="suffix">The suffix of the scaled down number.</param>
        /// <returns></returns>
        public static void GetFormatPair ( Double number, out Double scaledNumber, out String suffix )
        {
            if ( Double.IsInfinity ( number ) || Double.IsNaN ( number ) || number == 0D || number == -0D )
            {
                scaledNumber = number;
                suffix = "";
                return;
            }

            // Fast path for non-scaled numbers
            if ( 1 <= number && number < Kilo )
            {
                scaledNumber = number;
                suffix = "";
                return;
            }
            if ( number >= Yotta )
            {
                scaledNumber = number / Yotta;
                suffix = "Y";
                return;
            }
            else if ( number >= Zetta )
            {
                scaledNumber = number / Zetta;
                suffix = "Z";
                return;
            }
            else if ( number >= Exa )
            {
                scaledNumber = number / Exa;
                suffix = "E";
                return;
            }
            else if ( number >= Peta )
            {
                scaledNumber = number / Peta;
                suffix = "P";
                return;
            }
            else if ( number >= Tera )
            {
                scaledNumber = number / Tera;
                suffix = "T";
                return;
            }
            else if ( number >= Giga )
            {
                scaledNumber = number / Giga;
                suffix = "G";
                return;
            }
            else if ( number >= Mega )
            {
                scaledNumber = number / Mega;
                suffix = "M";
                return;
            }
            else if ( number >= Kilo )
            {
                scaledNumber = number / Kilo;
                suffix = "k";
                return;
            }
            else if ( number >= Milli )
            {
                scaledNumber = number / Milli;
                suffix = "m";
                return;
            }
            else if ( number >= Micro )
            {
                scaledNumber = number / Micro;
                suffix = "μ";
                return;
            }
            else if ( number >= Nano )
            {
                scaledNumber = number / Nano;
                suffix = "n";
                return;
            }
            else if ( number >= Pico )
            {
                scaledNumber = number / Pico;
                suffix = "p";
                return;
            }
            else if ( number >= Femto )
            {
                scaledNumber = number / Femto;
                suffix = "f";
                return;
            }
            else if ( number >= Atto )
            {
                scaledNumber = number / Atto;
                suffix = "a";
                return;
            }
            else if ( number >= Zepto )
            {
                scaledNumber = number / Zepto;
                suffix = "z";
                return;
            }
            else if ( number >= Yocto )
            {
                scaledNumber = number / Yocto;
                suffix = "y";
                return;
            }
            else
            {
                scaledNumber = number;
                suffix = "";
                return;
            }
        }

        /// <summary>
        /// Formats a <paramref name="number" /> with its SI prefix appended to it.
        /// </summary>
        /// <param name="number">The number to be formatted.</param>
        /// <returns>The formatted string with the reduced number and the SI prefix.</returns>
        public static String Format ( Double number )
        {
            GetFormatPair ( number, out var scaled, out var suffix );
            return $"{scaled:0.##} {suffix}";
        }

        /// <summary>
        /// <inheritdoc cref="Format(Double)" /> The default format is <c>{0:0.##} {1}</c>.
        /// </summary>
        /// <param name="number"><inheritdoc cref="Format(Double)" /></param>
        /// <param name="format">
        /// The format string. Must contain two format placeholders. The default value is
        /// <c>{0:0.##} {1}</c>.
        /// </param>
        /// <returns><inheritdoc cref="Format(Double)" /></returns>
        [SuppressMessage ( "Globalization", "CA1305:Specify IFormatProvider", Justification = "There's already another overload for this." )]
        public static String Format ( Double number, String format )
        {
            GetFormatPair ( number, out var scaled, out var suffix );
            return String.Format ( format, scaled, suffix );
        }

        /// <summary>
        /// <inheritdoc cref="Format(Double, String)" />
        /// </summary>
        /// <param name="number"><inheritdoc cref="Format(Double, String)" /></param>
        /// <param name="format"><inheritdoc cref="Format(Double, String)" /></param>
        /// <param name="provider">
        /// <inheritdoc cref="String.Format(IFormatProvider, String, Object, Object)" />
        /// </param>
        /// <returns><inheritdoc cref="Format(Double, String)" /></returns>
        public static String Format ( Double number, String format, IFormatProvider provider )
        {
            GetFormatPair ( number, out var scaled, out var suffix );
            return String.Format ( provider, format, scaled, suffix );
        }

        /// <summary>
        /// Parses a SI number in the format <c>(0|.0|0.0)
        /// (y|z|a|f|p|n|u|μ|m||k|M|G|T|P|E|Z|Y)\w*</c>. Might suffer from precision loss.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="number">The number that the input string represents.</param>
        /// <returns>The number that the input string represents.</returns>
        public static Boolean TryParse ( String input, out Double number )
        {
            Match match = _floatParseRegex.Match ( input );
            if ( !match.Success )
            {
                number = default;
                return false;
            }

            if ( !Double.TryParse ( match.Groups["number"].Value, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsed ) )
            {
                number = default;
                return false;
            }

            switch ( match.Groups["suffix"].Value )
            {
                case "y":
                    number = parsed * Yocto;
                    return true;

                case "z":
                    number = parsed * Zepto;
                    return true;

                case "a":
                    number = parsed * Atto;
                    return true;

                case "f":
                    number = parsed * Femto;
                    return true;

                case "p":
                    number = parsed * Pico;
                    return true;

                case "n":
                    number = parsed * Nano;
                    return true;

                case "u":
                case "μ":
                    number = parsed * Micro;
                    return true;

                case "m":
                    number = parsed * Milli;
                    return true;

                case "":
                    number = parsed;
                    return true;

                case "k":
                    number = parsed * Kilo;
                    return true;

                case "M":
                    number = parsed * Mega;
                    return true;

                case "G":
                    number = parsed * Giga;
                    return true;

                case "T":
                    number = parsed * Tera;
                    return true;

                case "P":
                    number = parsed * Peta;
                    return true;

                case "E":
                    number = parsed * Exa;
                    return true;

                case "Z":
                    number = parsed * Zetta;
                    return true;

                case "Y":
                    number = parsed * Yotta;
                    return true;

                default:
                    number = default;
                    return false;
            }
        }

        /// <summary>
        /// <inheritdoc cref="TryParse(String, out Double)" />
        /// </summary>
        /// <param name="input"><inheritdoc cref="TryParse(String, out Double)" /></param>
        /// <returns>
        /// <inheritdoc cref="TryParse(String, out Double)" path="/param[@name='number']" />
        /// </returns>
        public static Double Parse ( String input )
        {
            if ( TryParse ( input, out var number ) )
                return number;
            throw new FormatException ( "The input string was in an unknown format." );
        }
    }
}