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

namespace GUtils.Timing
{
    /// <summary>
    /// A <see cref="TimingLogger" /> that outputs to the Console
    /// </summary>
    public class ConsoleTimingLogger : TimingLogger
    {
        /// <summary>
        /// Initializes a new console timing logger
        /// </summary>
        public ConsoleTimingLogger ( )
        {
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="str"></param>
        protected override void WriteInternal ( String str ) => Console.Write ( str );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        protected override void WriteInternal ( String str, ConsoleColor color )
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write ( str );
            Console.ForegroundColor = c;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="line"></param>
        protected override void WriteLineInternal ( String line ) => Console.WriteLine ( line );

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="line"></param>
        /// <param name="color"></param>
        protected override void WriteLineInternal ( String line, ConsoleColor color )
        {
            ConsoleColor c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine ( line );
            Console.ForegroundColor = c;
        }

        #region Extra Public I/O

        /// <summary>
        /// Reads an line of input from the console
        /// </summary>
        /// <returns></returns>
        public String ReadLine ( )
        {
            var line = Console.ReadLine ( );
            this.HasLineBeenPrefixed = false;
            return line;
        }

        #endregion Extra Public I/O
    }
}
