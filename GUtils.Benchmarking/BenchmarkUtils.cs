using System;
using System.Collections.Generic;

namespace GUtils.Benchmarking
{
	public class BenchmarkUtils
	{
		private readonly Stack<PrecisionStopwatch> Stopwatches = new Stack<PrecisionStopwatch> ( );

		public PrecisionStopwatch Pop ( )
		{
			PrecisionStopwatch sw = this.Stopwatches.Pop ( );
			sw.Stop ( );
			return sw;
		}

		public void Push ( )
		{
			var sw = new PrecisionStopwatch ( );
			this.Stopwatches.Push ( sw );
			sw.Start ( );
		}

		public Double Run ( Action fn, Int32 iterations = 1000 )
		{
			if ( fn == null )
				throw new ArgumentNullException ( nameof ( fn ) );

			var ms = 0.0;
			for ( var i = 0 ; i < iterations ; i++ )
			{
				this.Push ( );
				fn ( );
				ms += this.Pop ( ).ElapsedMicroseconds;
			}

			return ms / iterations;
		}
	}
}
