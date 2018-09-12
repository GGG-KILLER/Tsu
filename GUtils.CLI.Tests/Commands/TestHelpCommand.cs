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
