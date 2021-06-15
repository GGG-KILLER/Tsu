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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Tsu.Parsing.BBCode.Tree;
using static Tsu.Parsing.BBCode.Tree.BBNodeFactory;

namespace Tsu.Parsing.BBCode.Tests
{
    [TestClass]
    public class BBParserTests
    {
        private static void AssertParse(string code, BBNode expected)
        {
            Logger.LogMessage("Testing:  {0}", code);
            Logger.LogMessage("Expected: {0}", expected);
            var gotten = BBParser.Parse(code);
            Logger.LogMessage("Gotten: ");
            foreach (var got in gotten)
                Logger.LogMessage("    {0}", got);
            var firstGotten = gotten[0];
            Assert.IsTrue(expected.StructurallyEquals(firstGotten));
        }

        [TestMethod]
        public void ShouldParseTagWithoutValue() =>
            AssertParse("[b]a[/b]", Tag("b", Text("a")));

        [TestMethod]
        public void ShouldParseTagWithValue() =>
            AssertParse("[c=red]a[/c]", Tag("c", "red", Text("a")));

        [TestMethod]
        public void ShouldParseSelfClosingTagWithoutValue() =>
            AssertParse("[hr/]", SelfClosingTag("hr"));

        [TestMethod]
        public void ShouldParseSelfClosingTagWithValue() =>
            AssertParse("[youtube=x/]", SelfClosingTag("youtube", "x"));

        [TestMethod]
        public void ShouldParseEverythingAtOnce() =>
            AssertParse(
                "[b][c=red][youtube=youtube/][hr/][youtube=youtube/][/c][youtube=youtube/][hr/][youtube=youtube/][/b]",
                Tag("b",
                    Tag("c", "red",
                        SelfClosingTag("youtube", "youtube"),
                        SelfClosingTag("hr"),
                        SelfClosingTag("youtube", "youtube")
                    ),
                    SelfClosingTag("youtube", "youtube"),
                    SelfClosingTag("hr"),
                    SelfClosingTag("youtube", "youtube")
                )
            );
    }
}
