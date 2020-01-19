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
using GUtils.CLI.Commands;
using GUtils.CLI.Commands.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// We need backwards compatibility so disable obsolete stuff warnings
#pragma warning disable CS0618

namespace GUtils.CLI.Tests.Commands
{
    [TestClass]
    public class CommandTests
    {
        [Command ( "a" )]
        public static void Error01 ( out String a ) => a = "";

        [Command ( "b" )]
        public static void Error02 ( ref String b ) => b = "";

        [Command ( "c" )]
        public static void Error03 ( in String c ) => _ = c;

        [Command ( "e" )]
        public static void Error04 ( [JoinRestOfArguments] String a, String b ) => _ = a + b;

        [Command ( "f" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage ( "Design", "CA1062:Validate arguments of public methods", Justification = "Arguments are known" )]
        public static void Error05 ( String[] a, String b ) => _ = a[0] == b;

        [Command ( "g" ), HelpDescription ( "Sucess" )]
        public static String Success01 (
            [HelpDescription ( "First arg" )] String first,
            [HelpDescription ( "Second arg" )] Int32 second ) => first + second;

        [DataTestMethod]
        [DataRow ( typeof ( CommandTests ), nameof ( Error01 ) )]
        [DataRow ( typeof ( CommandTests ), nameof ( Error02 ) )]
        [DataRow ( typeof ( CommandTests ), nameof ( Error03 ) )]
        [DataRow ( typeof ( CommandTests ), nameof ( Error04 ) )]
        [DataRow ( typeof ( CommandTests ), nameof ( Error05 ) )]
        public void CtorShouldThrow ( Type type, String name )
            => Assert.ThrowsException<CommandDefinitionException> ( ( ) => new Command ( type.GetMethod ( name ), null ) );
    }
}
