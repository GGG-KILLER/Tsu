using System;
using System.Collections.Generic;

namespace GUtils.Benchmarking
{
	public class Benchmark
	{
		private readonly Stack<PrecisionStopwatch> Stopwatches = new Stack<PrecisionStopwatch> ( );

		public void Push ( )
		{
			var sw = new PrecisionStopwatch ( );
			this.Stopwatches.Push ( sw );
			sw.Start ( );
		}

		public PrecisionStopwatch Pop ( )
		{
			PrecisionStopwatch sw = this.Stopwatches.Pop ( );
			sw.Stop ( );
			return sw;
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

		public BenchmarkComparisonResult[] Compare ( params Action[] fns )
		{
			var result = new BenchmarkComparisonResult[fns.Length];

			for ( var i = 0 ; i < fns.Length ; i++ )
			{
				result[i] = new BenchmarkComparisonResult
				{
					Index = i,
					ElapsedMicroseconds = this.Run ( fns[i] )
				};
			}

			Array.Sort ( result, ( x, y ) =>
			{
				if ( x.ElapsedMicroseconds < y.ElapsedMicroseconds )
					return -1;
				else if ( x.ElapsedMicroseconds > y.ElapsedMicroseconds )
					return 1;
				return 0;
			} );

			return result;
		}
	}
}
