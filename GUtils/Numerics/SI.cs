using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.RegularExpressions;

namespace GUtils.Numerics
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
        /// <param name="number"></param>
        /// <returns></returns>
        public static (Double, String) GetFormatPair ( Double number )
        {
            if ( Double.IsInfinity ( number ) || Double.IsNaN ( number ) || number == 0D || number == -0D )
                return (number, "");

            // Fast path for non-scaled numbers
            if ( 1 <= number && number < Kilo )
                return (number, "");
            if ( number >= Yotta )
                return (number / Yotta, "Y");
            else if ( number >= Zetta )
                return (number / Zetta, "Z");
            else if ( number >= Exa )
                return (number / Exa, "E");
            else if ( number >= Peta )
                return (number / Peta, "P");
            else if ( number >= Tera )
                return (number / Tera, "T");
            else if ( number >= Giga )
                return (number / Giga, "G");
            else if ( number >= Mega )
                return (number / Mega, "M");
            else if ( number >= Kilo )
                return (number / Kilo, "k");
            else if ( number >= Milli )
                return (number / Milli, "m");
            else if ( number >= Micro )
                return (number / Micro, "μ");
            else if ( number >= Nano )
                return (number / Nano, "n");
            else if ( number >= Pico )
                return (number / Pico, "p");
            else if ( number >= Femto )
                return (number / Femto, "f");
            else if ( number >= Atto )
                return (number / Atto, "a");
            else if ( number >= Zepto )
                return (number / Zepto, "z");
            else if ( number >= Yocto )
                return (number / Yocto, "y");
            else
                return (number, "");
        }

        /// <summary>
        /// Formats a <paramref name="number" /> with its SI prefix appended to it.
        /// </summary>
        /// <param name="number">The number to be formatted.</param>
        /// <returns>The formatted string with the reduced number and the SI prefix.</returns>
        public static String Format ( Double number )
        {
            (var scaled, var suffix) = GetFormatPair ( number );
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
            (var scaled, var suffix) = GetFormatPair ( number );
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
            (var scaled, var suffix) = GetFormatPair ( number );
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