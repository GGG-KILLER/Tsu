/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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
using System.Diagnostics;

namespace GUtils.Timing
{
    // Hello and welcome to IDisposable abuse central, how can we
    // help you?
    public class TimingArea : IDisposable
    {
        protected readonly String _indent;
        protected readonly Stopwatch _stopwatch;
        protected readonly TimingArea _root;

        public TimingArea ( String name, TimingArea parent = null )
        {
            this._indent = parent != null ? parent._indent + '\t' : "";
            this._root = parent?._root ?? this;
            this._stopwatch = Stopwatch.StartNew ( );

            this.Log ( name, false );
            this.Log ( "{", false );
        }

        public TimingLine TimeLine ( String name ) => new TimingLine ( name, this );

        public virtual void Log ( Object Message, Boolean extraIndent = true ) => Console.WriteLine ( $"[{this._root._stopwatch.Elapsed}]{this._indent}{( extraIndent ? "\t" : "" )} {Message}" );

        public virtual void Dispose ( )
        {
            this.Log ( $"Total time elapsed: {Duration.Format ( this._stopwatch.ElapsedTicks )}" );
            this.Log ( "}", false );
            GC.SuppressFinalize ( this );
        }
    }
}
