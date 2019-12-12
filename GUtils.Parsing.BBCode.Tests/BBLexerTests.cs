using System;
using System.IO;
using System.Linq;
using GUtils.Parsing.BBCode.Lexing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUtils.Parsing.BBCode.Tests
{
    [TestClass]
    public class BBLexerTests
    {
        private static readonly BBToken LBracket = new BBToken ( BBTokenType.LBracket );
        private static readonly BBToken RBracket = new BBToken ( BBTokenType.RBracket );
        private static new readonly BBToken Equals = new BBToken ( BBTokenType.Equals );
        private static readonly BBToken Slash = new BBToken ( BBTokenType.Slash );

        private static void AssertTokenStream ( String str, params BBToken[] expectedTokens )
        {
            using var reader = new StringReader ( str );
            BBToken[] gottenTokens = BBLexer.Lex ( reader ).ToArray ( );

            foreach ( (BBToken expected, BBToken gotten) in expectedTokens.Zip ( gottenTokens, ( a, b ) => (a, b) ) )
                Assert.AreEqual ( expected, gotten );

            Assert.AreEqual ( expectedTokens.Length, gottenTokens.Length, "Got a different amount of tokens than expected" );
        }

        [TestMethod]
        public void ShouldLexBrackets ( ) =>
            AssertTokenStream ( "[][][]", LBracket, RBracket, LBracket, RBracket, LBracket, RBracket );

        [TestMethod]
        public void ShouldLexText ( ) =>
            AssertTokenStream ( "some=text/for=you", new BBToken ( "some=text/for=you" ) );

        [TestMethod]
        public void ShouldLexEmptyString ( ) =>
            AssertTokenStream ( "" );

        [TestMethod]
        public void ShouldLexTagWithoutValue ( ) =>
            AssertTokenStream ( "[a][/a]", LBracket, new BBToken ( "a" ), RBracket, LBracket, Slash, new BBToken ( "a" ), RBracket );

        [TestMethod]
        public void ShouldLexTagWithValue ( ) =>
            AssertTokenStream ( "[a=a][/a]", LBracket, new BBToken ( "a" ), Equals, new BBToken ( "a" ), RBracket, LBracket, Slash, new BBToken ( "a" ), RBracket );

        [TestMethod]
        public void ShouldLexSelfClosingTagWithoutValue ( ) =>
            AssertTokenStream ( "[a/]", LBracket, new BBToken ( "a" ), Slash, RBracket );

        [DataTestMethod]
        [DataRow ( "[[a]" )]
        [DataRow ( "[a[a]" )]
        [DataRow ( "[a=[a]" )]
        public void ShouldNotLex ( String text ) =>
            Assert.ThrowsException<FormatException> ( ( ) =>
            {
                using var reader = new StringReader ( text );
                BBLexer.Lex ( reader ).ToArray ( );
            } );
    }
}