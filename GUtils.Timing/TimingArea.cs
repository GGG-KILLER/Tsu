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
using System.Diagnostics;

// Hello and welcome to IDisposable abuse central, how can we help you?
namespace GUtils.Timing
{
    /// <summary>
    /// Represents a section of code being timed, by default
    /// outputs everything to the console
    /// </summary>
    public class TimingArea : IDisposable
    {
        /// <summary>
        /// The cached indentation level of this area
        /// </summary>
        protected readonly String _indent;

        /// <summary>
        /// The <see cref="Stopwatch" /> measuring the elapsed
        /// time of this area
        /// </summary>
        protected readonly Stopwatch _stopwatch;

        /// <summary>
        /// The root <see cref="TimingArea" />
        /// </summary>
        protected readonly TimingArea _root;

        /// <summary>
        /// Initializes this <see cref="TimingArea" /> increasing
        /// the indentation level of the parent and printing the
        /// name of this timing area
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        public TimingArea ( String name, TimingArea parent = null )
        {
            this._indent = parent != null ? parent._indent + '\t' : "";
            this._root = parent?._root ?? this;
            this._stopwatch = Stopwatch.StartNew ( );

            this.Log ( name, false );
            this.Log ( "{", false );
        }

        /// <summary>
        /// Initializes a new <see cref="TimingLine" />
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TimingLine TimeLine ( String name ) => new TimingLine ( name, this );

        /// <summary>
        /// Outputs an indented line prefixed by the elapsed time
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="extraIndent"></param>
        public virtual void Log ( Object Message, Boolean extraIndent = true ) => Console.WriteLine ( $"[{this._root._stopwatch.Elapsed}]{this._indent}{( extraIndent ? "\t" : "" )} {Message}" );

        /// <summary>
        /// Ends this <see cref="TimingArea" /> showing the total
        /// time elapsed and ends the indented area
        /// </summary>
        public virtual void Dispose ( )
        {
            this.Log ( $"Total time elapsed: {Duration.Format ( this._stopwatch.ElapsedTicks )}" );
            this.Log ( "}", false );
        }
    }
}
