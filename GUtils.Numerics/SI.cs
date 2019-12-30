using System;

namespace GUtils.Numerics
{
    /// <summary>
    /// A subset of the International System of Units Prefixes that only contains the prefixes that are
    /// powers of 1000
    /// </summary>
    public static class SI
    {
        /// <summary>
        /// A yotta (Y)
        /// </summary>
        public const Double Yotta = Zetta * 1000;

        /// <summary>
        /// A zetta (Z)
        /// </summary>
        public const Double Zetta = Exa   * 1000;

        /// <summary>
        /// An exa (E)
        /// </summary>
        public const Double Exa   = Peta  * 1000;

        /// <summary>
        /// A peta (P)
        /// </summary>
        public const Double Peta  = Tera  * 1000;

        /// <summary>
        /// A tera (T)
        /// </summary>
        public const Double Tera  = Giga  * 1000;

        /// <summary>
        /// A giga (G)
        /// </summary>
        public const Double Giga  = Mega  * 1000;

        /// <summary>
        /// A mega (M)
        /// </summary>
        public const Double Mega  = Kilo  * 1000;

        /// <summary>
        /// A kilo (K)
        /// </summary>
        public const Double Kilo  = 1000;

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
        public const Double Nano  = Micro * 0.001;

        /// <summary>
        /// A pico (p)
        /// </summary>
        public const Double Pico  = Nano  * 0.001;

        /// <summary>
        /// A femto (f)
        /// </summary>
        public const Double Femto = Pico  * 0.001;

        /// <summary>
        /// An atto (a)
        /// </summary>
        public const Double Atto  = Femto * 0.001;

        /// <summary>
        /// A zepto (z)
        /// </summary>
        public const Double Zepto = Atto  * 0.001;

        /// <summary>
        /// A yocto (y)
        /// </summary>
        public const Double Yocto = Zepto * 0.001;

        private static readonly String[] _prefixes =
        {
            /* -8 */ "y",
            /* -7 */ "z",
            /* -6 */ "a",
            /* -5 */ "f",
            /* -4 */ "p",
            /* -3 */ "n",
            /* -2 */ "μ",
            /* -1 */ "m",
            /*  0 */ "\uFFFF",
            /*  1 */ "k",
            /*  2 */ "M",
            /*  3 */ "G",
            /*  4 */ "T",
            /*  5 */ "P",
            /*  6 */ "E",
            /*  7 */ "Z",
            /*  8 */ "Y",
        };

        /// <summary>
        /// Returns the pair of objects used in suffixing a SI number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static (Double, String) GetFormatPair ( Double number )
        {
            var pow = ( Int32 ) Math.Max ( Math.Min ( Math.Log ( number, 1000d ), 8 ), -8 );
            if ( pow == 0 )
                return (number, "");

            return (number / Math.Pow ( 1000, pow ), _prefixes[8 + pow]);
        }

        /// <summary>
        /// Formats a <paramref name="number" /> with its SI suffix according to the
        /// <paramref name="format" />
        /// </summary>
        /// <param name="number">The number to be formatted</param>
        /// <param name="format">The format string. Must contain two format locations/slots.</param>
        /// <returns></returns>
        public static String Format ( Double number, String format )
        {
            (var scaled, var suffix) = GetFormatPair ( number );
            return String.Format ( format, scaled, suffix );
        }

        /// <summary>
        /// Formats a <paramref name="number" /> with it's SI prefix appended to it
        /// </summary>
        /// <param name="number">The number to format</param>
        /// <returns></returns>
        public static String Format ( Double number ) =>
            Format ( number, "{0} {1}" );
    }
}
