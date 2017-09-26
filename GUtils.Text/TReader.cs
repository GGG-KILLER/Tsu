using System;
using System.Collections.Generic;
using System.Linq;

namespace GUtils.Text
{
	public class TReader<T> where T : class
	{
		private readonly T[] Value;

		public Int32 Position { get; protected set; }

		public Int32 Length => this.Value.Length - this.Position;

		public TReader ( IEnumerable<T> Value )
		{
			this.Value = Value.ToArray ( );
			this.Position = -1;
		}

		#region Basic Checking

		/// <summary>
		/// Checks wether we can move <paramref name="Delta" /> Tacters
		/// </summary>
		/// <param name="Delta">Amount of Tacters to move</param>
		/// <returns></returns>
		public Boolean CanMove ( Int32 Delta )
		{
			var newPos = this.Position + Delta;
			return -1 < newPos && newPos < this.Value.Length;
		}

		/// <summary>
		/// Checks whether the next Tacter after the
		/// <paramref name="dist" /> th Tacter is <paramref name="ch" />
		/// </summary>
		/// <param name="ch">The Tacter to find</param>
		/// <param name="dist">
		/// The distance at when to start looking for
		/// </param>
		/// <returns></returns>
		public Boolean IsNext ( T ch, Int32 dist = 1 )
		{
			return Peek ( dist ) == ch;
		}

		/// <summary>
		/// Checks whether <paramref name="a" /> is before <paramref name="b" />
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <returns></returns>
		public Boolean AIsBeforeB ( T a, T b )
		{
			var idxa = this.IndexOf ( a );
			var idxb = this.IndexOf ( b );
			return idxa != -1 && idxb != -1 && idxa < idxb;
		}

		#endregion Basic Checking

		#region Basic Movement

		/// <summary>
		/// Consumes the <paramref name="dist" /> th next Tacter
		/// </summary>
		/// <param name="dist"></param>
		/// <returns></returns>
		public T Read ( Int32 dist = 1 )
		{
			if ( !CanMove ( dist ) )
				return null;
			this.Position += dist;
			return this.Value[this.Position];
		}

		/// <summary>
		/// Returns the next <paramref name="dist" /> th Tacter
		/// </summary>
		/// <param name="dist"></param>
		/// <returns></returns>
		public T Peek ( Int32 dist = 1 )
		{
			if ( !CanMove ( dist ) )
				return null;
			return this.Value[this.Position + dist];
		}

		/// <summary>
		/// Returns the distance from <see cref="Position" /> +
		/// <paramref name="offset" /> to <paramref name="ch" />
		/// </summary>
		/// <param name="ch">Tacter to search for</param>
		/// <param name="offset">
		/// Distance from current position where to start
		/// searching for
		/// </param>
		/// <returns></returns>
		public Int32 IndexOf ( T ch, Int32 offset = 0 )
		{
			var idx = Array.IndexOf ( this.Value, ch, this.Position + offset );
			return idx == -1 ? -1 : idx - ( this.Position + offset );
		}

		/// <summary>
		/// Returns the index of a Tacter that passes the
		/// <paramref name="Filter" /> (NOT AN ABSOLUTE POSITION,
		/// IT IS RELATIVE TO <see cref="Position" />
		/// + <paramref name="offset" />)
		/// </summary>
		/// <param name="Filter">The filter function</param>
		/// <param name="offset">
		/// Offset from current opsition where to start searching from
		/// </param>
		/// <returns></returns>
		public Int32 IndexOf ( Func<T, Boolean> Filter, Int32 offset = 0 )
		{
			if ( Filter == null )
				throw new ArgumentNullException ( nameof ( Filter ) );

			for ( Int32 i = this.Position + offset ; i < this.Value.Length ; i++ )
				if ( Filter ( this.Value[i] ) )
					return i - ( this.Position + offset );
			return -1;
		}

		#endregion Basic Movement

		#region Advanced Movement

		#region T Based

		/// <summary>
		/// Reads until <paramref name="ch" /> is found
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public T[] ReadUntil ( T ch )
		{
			var idx = this.IndexOf ( ch );
			return idx == -1 ? null : this.ReadSeq ( idx );
		}

		/// <summary>
		/// Reads until <paramref name="Filter" /> is false
		/// </summary>
		/// <param name="Filter"></param>
		/// <returns></returns>
		public T[] ReadUntil ( Func<T, Boolean> Filter )
		{
			var idx = this.IndexOf ( Filter );
			return idx == -1 ? null : this.ReadSeq ( idx );
		}

		/// <summary>
		/// Reads until <paramref name="Filter" /> is false
		/// </summary>
		/// <param name="Filter"></param>
		/// <returns></returns>
		public T[] ReadUntil ( Func<T, T, Boolean> Filter )
		{
			if ( Filter == null ) throw new ArgumentNullException ( nameof ( Filter ) );
			var len = 0;
			while ( !Filter ( Peek ( len + 1 ), Peek ( len + 2 ) ) )
				len++;
			return ReadSeq ( len );
		}

		/// <summary>
		/// Reads until <paramref name="Filter" /> is false
		/// </summary>
		/// <param name="Filter"></param>
		/// <returns></returns>
		public T[] ReadWhile ( Func<T, Boolean> Filter )
		{
			if ( Filter == null ) throw new ArgumentNullException ( nameof ( Filter ) );
			var len = 0;
			while ( Filter ( Peek ( 1 + len ) ) )
				len++;
			return this.ReadSeq ( len );
		}

		/// <summary>
		/// Reads until <paramref name="Filter" /> is false
		/// </summary>
		/// <param name="Filter"></param>
		/// <returns></returns>
		public T[] ReadWhile ( Func<T, T, Boolean> Filter )
		{
			if ( Filter == null ) throw new ArgumentNullException ( nameof ( Filter ) );
			var len = 0;
			while ( Filter ( Peek ( 1 + len ), Peek ( 2 + len ) ) )
				len++;
			return this.ReadSeq ( len );
		}

		#endregion T Based

		/// <summary>
		/// Reads and returns a <see cref="T[]" /> of <paramref name="length" />
		/// </summary>
		/// <param name="length">Length of the T[] to read</param>
		/// <returns></returns>
		public T[] ReadSeq ( Int32 length )
		{
			if ( length == 0 )
				return new T[0];
			try
			{
				var arr = new T[length];
				Array.Copy ( this.Value, this.Position + 1, arr, 0, length );
				return arr;
			}
			finally
			{
				this.Position += length;
			}
		}

		#endregion Advanced Movement
	}
}
