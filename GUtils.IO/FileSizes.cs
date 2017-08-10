using System;
using System.Collections.Generic;
using System.Text;

namespace GUtils.IO
{
    public static class FileSizes
    {
        public const UInt64 B = 1024;
        public const UInt64 KiB = 1024 * B;
        public const UInt64 MiB = 1024 * KiB;
        public const UInt64 GiB = 1024 * MiB;
        public const UInt64 TiB = 1024 * GiB;
        public const UInt64 PiB = 1024 * TiB;

        private static readonly String[] _suffixes = new[] { "B", "KiB", "MiB", "GiB", "TiB", "PiB" };

        public static String Format ( UInt64 Size )
        {
            var i = ( Int32 ) Math.Round ( Math.Log ( Size, 1024 ) );
            return $"{Size / ( Math.Pow ( i, B ) )} {_suffixes[i]}";
        }
    }
}
