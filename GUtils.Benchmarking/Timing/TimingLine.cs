﻿using System;
using System.Diagnostics;

namespace GUtils.Benchmarking.Timing
{
	public class TimingLine : IDisposable
	{
		private readonly TimingArea parent;
		private readonly String name;
		private readonly Stopwatch stopwatch;

		// You aren't suposed to initialize this by yourself
		internal TimingLine ( String name, TimingArea parent )
		{
			this.name = name;
			this.parent = parent ?? throw new ArgumentNullException ( nameof ( parent ) );
			this.stopwatch = Stopwatch.StartNew ( );
		}

		public void Dispose ( )
		{
			this.parent.Log ( $"Time elapsed on {this.name}: {this.stopwatch.Elapsed}" );
			GC.SuppressFinalize ( this );
		}
	}
}
