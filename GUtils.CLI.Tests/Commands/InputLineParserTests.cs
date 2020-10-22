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
using System.Diagnostics.CodeAnalysis;
using GUtils.CLI.Commands;
using GUtils.CLI.Commands.Errors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace GUtils.CLI.Tests.Commands
{
    [TestClass]
    public class InputLineParserTests
    {
        [DataTestMethod, TestCategory ( "Complex Parse" )]

        #region Escapes

        [DataRow ( @"\a\b\f", "\a\b\f" )]
        [DataRow ( @"\a\\b\f", "\a\\b\f" )]
        [DataRow ( "\"a \\\" b\"", "a \" b" )]

        #endregion Escapes

        #region Space-separated stuff

        [DataRow ( @"hello world", "hello;world" )]
        [DataRow ( @"      hello         world       ", "hello;world" )]
        [DataRow ( @"\thello\t \tworld\t", "\thello\t;\tworld\t" )]

        #endregion Space-separated stuff

        #region Quoted stuff

        [DataRow ( "\"hello world\" 'how are you?'", "hello world;how are you?" )]
        [DataRow ( "'hello \" world' how are you \"\"", "hello \" world;how;are;you;" )]
        [DataRow ( "\"hello \\\" world\"", "hello \" world" )]
        [DataRow ( @"'\o150\b1100101\o154\x6c\x6f\x20\b1110111\o157\x72\o154\x64'", "hello world" )]

        #endregion Quoted stuff

        #region Rest "operator"

        [DataRow ( @"let\'s start by saying r:\o150\b1100101\o154\x6c\x6f\x20\b1110111\o157\x72\o154\x64 how are you?", "let's;start;by;saying;hello world how are you?" )]
        [DataRow ( "hello wor:ld how are you?", "hello;wor:ld;how;are;you?" )]
        [DataRow ( "hello world r:", "hello;world" )]
        [DataRow ( @"hello rr:\b1110111\o157\x72\o154\x64 yeah", @"hello;\b1110111\o157\x72\o154\x64 yeah" )]
        [DataRow ( @"rr:hello\x20world", "hello\\x20world" )]

        #endregion Rest "operator"

        [SuppressMessage ( "Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>" )]
        public void ParseShouldParse ( String input, String rawExpected )
        {
            try
            {
                var expected = rawExpected.Split ( ';' ); var i = 0;
                foreach ( var argument in InputLineParser.Parse ( input ) )
                {
                    Assert.AreEqual ( expected[i], argument, $"Argument #{i} doesn't match the expected value." );
                    i++;
                }
            }
            catch ( InputLineParseException ilpe )
            {
                Logger.LogMessage ( $"{ilpe.Message}:" );
                Logger.LogMessage ( input );
                Logger.LogMessage ( "{0}^", new String ( ' ', ilpe.Offset ) );
                throw;
            }
        }
    }
}