/*
 * Copyright © 2019 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the “Software”), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
 * the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace GUtils.Timing
{
    /// <summary>
    /// A micro profiler. Basically a tree of Stopwatches with associated names.
    /// </summary>
    [DebuggerDisplay ( "{Name}" )]
    [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Performance", "CA1815:Override equals and operator equals on value types", Justification = "There is no point to checking equality of two microprofilers" )]
    public readonly struct MicroProfiler : IDisposable
    {
        /// <summary>
        /// Instantiates and starts a new <see cref="MicroProfiler" />
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MicroProfiler StartNew ( String name )
        {
            var prof = new MicroProfiler(name);
            prof.Stopwatch.Start ( );
            return prof;
        }

        /// <summary>
        /// The name associated with this microprofiler
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// The list of child microprofilers
        /// </summary>
        public List<MicroProfiler> ChildResults { get; }

        /// <summary>
        /// The stopwatch used for timing of this microprofiler
        /// </summary>
        private readonly Stopwatch Stopwatch;

        /// <summary>
        /// The total milliseconds elapsed on this operation
        /// </summary>
        public Double ElapsedMilliseconds => this.Stopwatch.ElapsedTicks / Duration.TicksPerMillisecond;

        /// <summary>
        /// Initializes a new MicroProfiler with the given name.
        ///
        /// Does NOT start the internal stopwatch.
        /// </summary>
        /// <param name="name"></param>
        public MicroProfiler ( String name )
        {
            this.Name = name ?? throw new ArgumentNullException ( nameof ( name ) );
            this.ChildResults = new List<MicroProfiler> ( );
            this.Stopwatch = new Stopwatch ( );
        }

        /// <summary>
        /// Instantiates, starts and adds a new <see cref="MicroProfiler" /> with the provided
        /// <paramref name="name" /> to the <see cref="ChildResults" />.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MicroProfiler StartChild ( String name )
        {
            var res = new MicroProfiler(name);
            this.ChildResults.Add ( res );
            res.Stopwatch.Start ( );
            return res;
        }

        /// <summary>
        /// Starts the internal stopwatch
        /// </summary>
        public void Start ( ) => this.Stopwatch.Start ( );

        /// <summary>
        /// Restarts the internal stopwatch
        /// </summary>
        public void Restart ( ) => this.Stopwatch.Restart ( );

        /// <summary>
        /// Stops the internal stopwatch
        /// </summary>
        public void Stop ( ) => this.Stopwatch.Stop ( );

        /// <summary>
        /// Resets the internal stopwatch
        /// </summary>
        public void Reset ( ) => this.Stopwatch.Reset ( );

        /// <summary>
        /// Writes the tree of timings to the provided <paramref name="builder" />.
        /// </summary>
        /// <param name="builder"></param>
        public void WriteTreeString ( StringBuilder builder )
        {
            var x = 0UL;
            this.WriteTreeString ( builder, 0, ref x );
        }

        /// <summary>
        /// Checks if a bit is set in a 64-bit bit vector (implemented as an <see cref="UInt64" />).
        /// </summary>
        /// <param name="bitVector"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        private static Boolean IsBitSet ( UInt64 bitVector, Int32 bitIndex )
        {
            var bit = 1UL << (bitIndex - 1);
            return ( bitVector & bit ) == bit;
        }

        /// <summary>
        /// Sets a bit in a 64-bit bit vector (implemented as an <see cref="UInt64" />).
        /// </summary>
        /// <param name="bitVector"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        [MethodImpl ( MethodImplOptions.AggressiveInlining )]
        private static void SetBit ( ref UInt64 bitVector, Int32 bitIndex, Boolean value )
        {
            var bit = 1UL << (bitIndex - 1);
            if ( value )
            {
                bitVector |= bit;
            }
            else
            {
                bitVector &= ~bit;
            }
        }

        /// <summary>
        /// Recursively writes the tree containing all timings to the provided
        /// <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="StringBuilder" /> where all output will be written to.
        /// </param>
        /// <param name="depth">How deep we're in the <see cref="MicroProfiler" /> tree.</param>
        /// <param name="isLastFlagVec">
        /// This 64-bit bit vector stores whether an item at any given depth was the last children of its
        /// parent.
        /// </param>
        private void WriteTreeString ( StringBuilder builder, Int32 depth, ref UInt64 isLastFlagVec )
        {
            const String leadingItemPadding = "│  ";
            const String lastItemPadding = "   ";
            const String leadingItemPrefix = "├─ ";
            const String lastItemPrefix = "└─ ";

            if ( depth > 0 )
            {
                for ( var i = 1; i < depth; i++ )
                {
                    builder.Append ( IsBitSet ( isLastFlagVec, i ) ? lastItemPadding : leadingItemPadding );
                }

                builder.Append ( IsBitSet ( isLastFlagVec, depth ) ? lastItemPrefix : leadingItemPrefix );
            }
            builder.AppendLine ( $"{this.Name}: {Duration.Format ( this.Stopwatch.ElapsedTicks )}" );

            SetBit ( ref isLastFlagVec, depth + 1, false );
            for ( var i = 0; i < this.ChildResults.Count; i++ )
            {
                if ( i == this.ChildResults.Count - 1 )
                {
                    SetBit ( ref isLastFlagVec, depth + 1, true );
                }

                this.ChildResults[i].WriteTreeString ( builder, depth + 1, ref isLastFlagVec );
            }
        }

        /// <summary>
        /// Outputs the tree of <see cref="MicroProfiler" /> s as an ASCII-like tree.
        /// </summary>
        /// <remarks>Uses the followign unicode characters: │, ├, ─ and └</remarks>
        /// <returns></returns>
        public override String ToString ( )
        {
            var sb = new StringBuilder();
            this.WriteTreeString ( sb );
            return sb.ToString ( );
        }

        void IDisposable.Dispose ( ) => this.Stop ( );
    }
}
