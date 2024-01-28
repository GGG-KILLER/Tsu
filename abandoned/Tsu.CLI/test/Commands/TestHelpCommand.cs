// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tsu.CLI.Commands;
using Tsu.CLI.Commands.Help;

namespace Tsu.CLI.Tests.Commands
{
    internal enum WriteType
    {
        Char, String, StringLine,
        CharLine
    }

    internal class TestHelpCommand : ConsoleHelpCommand
    {
        public readonly Queue<(WriteType, object)> ExpectedWritesQueue;

        public TestHelpCommand(CompiledCommandManager manager) : base(manager)
        {
            ExpectedWritesQueue = new Queue<(WriteType, object)>();
        }

        public void AddWrite(char ch) => ExpectedWritesQueue.Enqueue((WriteType.Char, ch));
        public void AddWrite(string str) => ExpectedWritesQueue.Enqueue((WriteType.String, str));
        public void AddLine(string str) => ExpectedWritesQueue.Enqueue((WriteType.StringLine, str));

        private void CheckWrite(WriteType actualType, object actualValue)
        {
            (var expectedType, var expectedValue) = ExpectedWritesQueue.Dequeue();
            Assert.AreEqual(expectedType, actualType);
            Assert.AreEqual(expectedValue, actualValue);
        }

        protected override void Write(char ch)
        {
            base.Write(ch);
            CheckWrite(WriteType.Char, ch);
        }

        protected override void Write(string text)
        {
            base.Write(text);
            CheckWrite(WriteType.String, text);
        }

        protected override void WriteLine(char ch)
        {
            base.WriteLine(ch);
            CheckWrite(WriteType.CharLine, ch);
        }

        protected override void WriteLine(string line)
        {
            base.WriteLine(line);
            CheckWrite(WriteType.StringLine, line);
        }
    }
}
