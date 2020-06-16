using System;
using GUtils.Parsing.BBCode.Tree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using static GUtils.Parsing.BBCode.Tree.BBNodeFactory;

namespace GUtils.Parsing.BBCode.Tests
{
    [TestClass]
    public class BBParserTests
    {
        private static void AssertParse ( String code, BBNode expected )
        {
            Logger.LogMessage ( "Testing:  {0}", code );
            Logger.LogMessage ( "Expected: {0}", expected );
            BBNode[] gotten = BBParser.Parse ( code );
            Logger.LogMessage ( "Gotten: " );
            foreach ( BBNode got in gotten )
                Logger.LogMessage ( "    {0}", got );
            BBNode firstGotten = gotten[0];
            Assert.IsTrue ( expected.StructurallyEquals ( firstGotten ) );
        }

        [TestMethod]
        public void ShouldParseTagWithoutValue ( ) =>
            AssertParse ( "[b]a[/b]", Tag ( "b", Text ( "a" ) ) );

        [TestMethod]
        public void ShouldParseTagWithValue ( ) =>
            AssertParse ( "[c=red]a[/c]", Tag ( "c", "red", Text ( "a" ) ) );

        [TestMethod]
        public void ShouldParseSelfClosingTagWithoutValue ( ) =>
            AssertParse ( "[hr/]", SelfClosingTag ( "hr" ) );

        [TestMethod]
        public void ShouldParseSelfClosingTagWithValue ( ) =>
            AssertParse ( "[youtube=x/]", SelfClosingTag ( "youtube", "x" ) );

        [TestMethod]
        public void ShouldParseEverythingAtOnce ( ) =>
            AssertParse (
                "[b][c=red][youtube=youtube/][hr/][youtube=youtube/][/c][youtube=youtube/][hr/][youtube=youtube/][/b]",
                Tag ( "b",
                    Tag ( "c", "red",
                        SelfClosingTag ( "youtube", "youtube" ),
                        SelfClosingTag ( "hr" ),
                        SelfClosingTag ( "youtube", "youtube" )
                    ),
                    SelfClosingTag ( "youtube", "youtube" ),
                    SelfClosingTag ( "hr" ),
                    SelfClosingTag ( "youtube", "youtube" )
                )
            );
    }
}
