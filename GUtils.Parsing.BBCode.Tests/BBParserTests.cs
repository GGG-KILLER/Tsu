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
        private void AssertParse ( String code, BBNode expected )
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
            this.AssertParse ( "[b]a[/b]", Tag ( "b", Text ( "a" ) ) );

        [TestMethod]
        public void ShouldParseTagWithValue ( ) =>
            this.AssertParse ( "[c=red]a[/c]", Tag ( "c", "red", Text ( "a" ) ) );

        [TestMethod]
        public void ShouldParseSelfClosingTagWithoutValue ( ) =>
            this.AssertParse ( "[hr/]", SelfClosingTag ( "hr" ) );

        [TestMethod]
        public void ShouldParseSelfClosingTagWithValue ( ) =>
            this.AssertParse ( "[youtube=x/]", SelfClosingTag ( "youtube", "x" ) );

        [TestMethod]
        public void ShouldParseEverythingAtOnce ( ) =>
            this.AssertParse (
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
