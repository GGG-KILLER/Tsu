using System;

namespace GUtils.IO
{
	public static class FileSize
	{
		public const Int64 KiB = 1024;
		public const Int64 MiB = 1024 * KiB;
		public const Int64 GiB = 1024 * MiB;
		public const Int64 TiB = 1024 * GiB;
		public const Int64 PiB = 1024 * TiB;

		private static readonly String[] _suffixes = new[] { "B", "KiB", "MiB", "GiB", "TiB", "PiB" };

		public static String Format ( Int64 Size )
		{
			var i = ( Int32 ) Math.Floor ( Math.Log ( Size, 1024 ) );
			return $"{Size / Math.Pow ( 1024, i )} {_suffixes[i]}";
		}

		public static String Format ( Double Size )
		{
			var i = ( Int32 ) Math.Floor ( Math.Log ( Size, 1024 ) );
			return $"{Size / Math.Pow ( 1024, i )} {_suffixes[i]}";
		}

		public static String Format ( Int64 Size, Int32 decimals )
		{
			var i = ( Int32 ) Math.Floor ( Math.Log ( Size, 1024 ) );
			return $"{Math.Round ( Size / Math.Pow ( 1024, i ), decimals )} {_suffixes[i]}";
		}

		public static String Format ( Double Size, Int32 decimals )
		{
			var i = ( Int32 ) Math.Floor ( Math.Log ( Size, 1024 ) );
			return $"{Math.Round ( Size / Math.Pow ( 1024, i ), decimals )} {_suffixes[i]}";
		}
	}
}
