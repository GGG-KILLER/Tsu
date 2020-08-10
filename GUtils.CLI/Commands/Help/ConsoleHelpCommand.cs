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

namespace GUtils.CLI.Commands.Help
{
    /// <summary>
    /// The console help command provider class
    /// </summary>
    public class ConsoleHelpCommand : HelpCommand
    {
        /// <summary>
        /// Initializes the default help command class
        /// </summary>
        /// <param name="manager"></param>
        public ConsoleHelpCommand ( BaseCommandManager manager ) : base ( manager )
        {
        }

        #region Write/WriteLine

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="ch"></param>
        protected override void Write ( Char ch ) => Console.Write ( ch );

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="text"></param>
        protected override void Write ( String text ) => Console.Write ( text );

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="ch"></param>
        protected override void WriteLine ( Char ch ) => Console.WriteLine ( ch );

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="line"></param>
        protected override void WriteLine ( String line ) => Console.WriteLine ( line );

        #endregion Write/WriteLine
    }
}