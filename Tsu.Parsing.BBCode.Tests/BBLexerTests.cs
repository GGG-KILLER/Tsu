// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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

using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tsu.Parsing.BBCode.Lexing;

namespace Tsu.Parsing.BBCode.Tests
{
    [TestClass]
    public class BBLexerTests
    {
        private static readonly BBToken LBracket = new BBToken(BBTokenType.LBracket);
        private static readonly BBToken RBracket = new BBToken(BBTokenType.RBracket);
        private static new readonly BBToken Equals = new BBToken(BBTokenType.Equals);
        private static readonly BBToken Slash = new BBToken(BBTokenType.Slash);

        private static void AssertTokenStream(string str, params BBToken[] expectedTokens)
        {
            using var reader = new StringReader(str);
            var gottenTokens = BBLexer.Lex(reader).ToArray();

            foreach ((var expected, var gotten) in expectedTokens.Zip(gottenTokens, (a, b) => (a, b)))
                Assert.AreEqual(expected, gotten);

            Assert.AreEqual(expectedTokens.Length, gottenTokens.Length, "Got a different amount of tokens than expected");
        }

        [TestMethod]
        public void ShouldLexBrackets() =>
            AssertTokenStream("[][][]", LBracket, RBracket, LBracket, RBracket, LBracket, RBracket);

        [TestMethod]
        public void ShouldLexText() =>
            AssertTokenStream("some=text/for=you", new BBToken("some=text/for=you"));

        [TestMethod]
        public void ShouldLexEmptyString() =>
            AssertTokenStream("");

        [TestMethod]
        public void ShouldLexTagWithoutValue() =>
            AssertTokenStream("[a][/a]", LBracket, new BBToken("a"), RBracket, LBracket, Slash, new BBToken("a"), RBracket);

        [TestMethod]
        public void ShouldLexTagWithValue() =>
            AssertTokenStream("[a=a][/a]", LBracket, new BBToken("a"), Equals, new BBToken("a"), RBracket, LBracket, Slash, new BBToken("a"), RBracket);

        [TestMethod]
        public void ShouldLexSelfClosingTagWithoutValue() =>
            AssertTokenStream("[a/]", LBracket, new BBToken("a"), Slash, RBracket);

        [DataTestMethod]
        [DataRow("[[a]")]
        [DataRow("[a[a]")]
        [DataRow("[a=[a]")]
        public void ShouldNotLex(string text) =>
            Assert.ThrowsException<FormatException>(() =>
          {
              using var reader = new StringReader(text);
              _ = BBLexer.Lex(reader).ToArray();
          });
    }
}