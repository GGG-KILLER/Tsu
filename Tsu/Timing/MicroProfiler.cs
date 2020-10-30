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
using System.Text;
using Tsu.Buffers;
using Tsu.Numerics;

namespace Tsu.Timing
{
    /// <summary>
    /// A micro profiler. Basically a tree of Stopwatches with associated names.
    /// </summary>
    [DebuggerDisplay ( "{" + nameof ( GetDebuggerDisplay ) + "(),nq}" )]
    public sealed class MicroProfiler : IDisposable
    {
        /// <summary>
        /// Instantiates and starts a new <see cref="MicroProfiler" />
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MicroProfiler StartNew ( String name )
        {
            var prof = new MicroProfiler ( name );
            prof._stopwatch.Start ( );
            return prof;
        }

        /// <summary>
        /// The list of child profilers.
        /// </summary>
        private readonly List<MicroProfiler> _childProfilers;

        /// <summary>
        /// The stopwatch used for timing of this micro profiler.
        /// </summary>
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// The name associated with this microprofiler
        /// </summary>
        public String Name { get; }

        /// <summary>
        /// The list of child microprofilers
        /// </summary>
        public IReadOnlyList<MicroProfiler> ChildProfilers => this._childProfilers;

        /// <summary>
        /// The total milliseconds elapsed on this operation
        /// </summary>
        public Double ElapsedMilliseconds => this._stopwatch.ElapsedTicks / Duration.TicksPerMillisecond;

        /// <summary>
        /// Initializes a new MicroProfiler with the given name.
        ///
        /// Does NOT start the internal stopwatch.
        /// </summary>
        /// <param name="name"></param>
        public MicroProfiler ( String name )
        {
            this.Name = name ?? throw new ArgumentNullException ( nameof ( name ) );
            this._childProfilers = new List<MicroProfiler> ( );
            this._stopwatch = new Stopwatch ( );
        }

        /// <summary>
        /// Instantiates, starts and adds a new <see cref="MicroProfiler" /> with the provided
        /// <paramref name="name" /> to the <see cref="ChildProfilers" />.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MicroProfiler StartChild ( String name )
        {
            MicroProfiler res = StartNew ( name );
            this._childProfilers.Add ( res );
            return res;
        }

        /// <summary>
        /// Starts the internal stopwatch
        /// </summary>
        public void Start ( ) => this._stopwatch.Start ( );

        /// <summary>
        /// Restarts the internal stopwatch
        /// </summary>
        public void Restart ( ) => this._stopwatch.Restart ( );

        /// <summary>
        /// Stops the internal stopwatch
        /// </summary>
        public void Stop ( ) => this._stopwatch.Stop ( );

        /// <summary>
        /// Resets the internal stopwatch
        /// </summary>
        public void Reset ( ) => this._stopwatch.Reset ( );

        /// <summary>
        /// Writes the tree of timings to the provided <paramref name="builder" />.
        /// </summary>
        /// <param name="builder"></param>
        public void WriteTreeString ( StringBuilder builder )
        {
            if ( builder is null )
                throw new ArgumentNullException ( nameof ( builder ) );

            this.WriteTreeString ( builder, 0, new VariableLengthBitVector ( ) );
        }

        /// <summary>
        /// Outputs the tree of <see cref="MicroProfiler" /> s as an ASCII-like tree.
        /// </summary>
        /// <remarks>Uses the followign unicode characters: │, ├, ─ and └</remarks>
        /// <returns></returns>
        public override String ToString ( )
        {
            var sb = new StringBuilder ( );
            this.WriteTreeString ( sb );
            return sb.ToString ( );
        }

        /// <inheritdoc/>
        public void Dispose ( ) => this.Stop ( );

        private String GetDebuggerDisplay ( ) => this.ToString ( );

        /// <summary>
        /// Recursively writes the tree containing all timings to the provided
        /// <paramref name="builder" />.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="StringBuilder" /> where all output will be written to.
        /// </param>
        /// <param name="depth">How deep we're in the <see cref="MicroProfiler" /> tree.</param>
        /// <param name="isLastBitVector">
        /// This bit vector stores whether an item at any given depth was the last children of its
        /// parent.
        /// </param>
        private void WriteTreeString ( StringBuilder builder, Int32 depth, VariableLengthBitVector isLastBitVector )
        {
            if ( depth > 0 )
            {
                for ( var i = 0; i < depth - 1; i++ )
                {
                    builder.Append ( isLastBitVector[i] ? ' ' : '│' )
                           .Append ( "  " );
                }

                builder.Append ( isLastBitVector[depth - 1] ? '└' : '├' )
                       .Append ( "─ " );
            }
            builder.AppendLine ( $"{this.Name}: {Duration.Format ( this._stopwatch.ElapsedTicks )}" );

            depth++;
            isLastBitVector[depth] = false;
            List<MicroProfiler> childResults = this._childProfilers;
            for ( var i = 0; i < childResults.Count; i++ )
            {
                if ( i == childResults.Count - 1 )
                {
                    isLastBitVector[depth] = true;
                }

                childResults[i].WriteTreeString ( builder, depth, isLastBitVector );
            }
        }
    }
}
