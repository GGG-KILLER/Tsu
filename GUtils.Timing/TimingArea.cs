using System;
using System.Diagnostics;

namespace GUtils.Timing
{
	public class TimingArea : IDisposable
	{
		private readonly String _indent;
		private readonly Stopwatch _stopwatch;
		private readonly TimingArea _root;

		public TimingArea ( String name, TimingArea parent = null )
		{
			this._indent = parent != null ? parent._indent + '\t' : "";
			this._root = parent?._root ?? this;
			this._stopwatch = Stopwatch.StartNew ( );

			this.Log ( name, false );
			this.Log ( "{", false );
		}

		public TimingLine TimeLine ( String name ) => new TimingLine ( name, this );

		public virtual void Log ( Object Message, Boolean indentIn = true )
		{
			Console.WriteLine ( $"{this._indent}{( indentIn ? "\t" : "" )}[{this._root._stopwatch.Elapsed}] {Message}" );
		}

		public void Dispose ( )
		{
			this.Log ( $"Final timing: {this._stopwatch.Elapsed}" );
			this.Log ( "}", false );
			GC.SuppressFinalize ( this );
		}
	}
}
