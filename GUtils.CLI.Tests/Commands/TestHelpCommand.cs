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
using System.Collections.Generic;
using GUtils.CLI.Commands;
using GUtils.CLI.Commands.Help;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUtils.CLI.Tests.Commands
{
    internal enum WriteType
    {
        Char, String, Line
    }

    internal class TestHelpCommand : DefaultHelpCommand
    {
        public readonly Queue<(WriteType, Object)> ExpectedWritesQueue;

        public TestHelpCommand ( in CommandManager manager ) : base ( manager )
        {
            this.ExpectedWritesQueue = new Queue<(WriteType, Object)> ( );
        }

        public void AddWrite ( in Char ch ) => this.ExpectedWritesQueue.Enqueue ( (WriteType.Char, ch) );
        public void AddWrite ( in String str ) => this.ExpectedWritesQueue.Enqueue ( (WriteType.String, str) );
        public void AddLine ( in String str ) => this.ExpectedWritesQueue.Enqueue ( (WriteType.Line, str) );

        private void CheckWrite ( in WriteType actualType, in Object actualValue )
        {
            (WriteType expectedType, var expectedValue) = this.ExpectedWritesQueue.Dequeue ( );
            Assert.AreEqual ( expectedType, actualType );
            Assert.AreEqual ( expectedValue, actualValue );
        }

        protected override void Write ( in Char ch )
        {
            base.Write ( ch );
            this.CheckWrite ( WriteType.Char, ch );
        }

        protected override void Write ( in String text )
        {
            base.Write ( text );
            this.CheckWrite ( WriteType.String, text );
        }
        protected override void WriteLine ( in String line )
        {
            base.WriteLine ( line );
            this.CheckWrite ( WriteType.Line, line );
        }
    }
}
