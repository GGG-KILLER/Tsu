using System;
using System.IO;
using GUtils.Parsing.BBCode.Lexing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GUtils.Parsing.BBCode.Tests
{
    [TestClass]
    public class BBLexerTests
    {
        private static BBToken Token ( BBTokenType tokenType, String value ) =>
            new BBToken ( tokenType, value );

        private static BBToken Text ( String value ) =>
            Token ( BBTokenType.Text, value );

        private static readonly BBToken LBracket = Token ( BBTokenType.LBracket, "[" );
        private static readonly BBToken RBracket = Token ( BBTokenType.RBracket, "]" );
        private static new readonly BBToken Equals = Token ( BBTokenType.Equals , "=" );
        private static readonly BBToken Slash = Token ( BBTokenType.Slash, "/" );

        private static void AssertTokenStream ( String str, params BBToken[] tokens )
        {
            using ( var reader = new StringReader ( str ) )
            {
                var lexer = new BBLexer ( reader );
                var i = 0;
                while ( reader.Peek ( ) != -1 )
                    Assert.AreEqual ( tokens[i++], lexer.NextToken ( ) );
            }
        }

        [TestMethod]
        public void ShouldLexBrackets ( ) =>
            AssertTokenStream ( "[][][]", LBracket, RBracket, LBracket, RBracket, LBracket, RBracket );

        [TestMethod]
        public void ShouldLexText ( ) =>
            AssertTokenStream ( "some=text/for=you", Text ( "some=text/for=you" ) );

        [TestMethod]
        public void ShouldLexEmptyString ( ) =>
            AssertTokenStream ( "" );

        [TestMethod]
        public void ShouldLexTagWithoutValue ( ) =>
            AssertTokenStream ( "[a][/a]", LBracket, Text ( "a" ), RBracket, LBracket, Slash, Text ( "a" ), RBracket );

        [TestMethod]
        public void ShouldLexTagWithValue ( ) =>
            AssertTokenStream ( "[a=a][/a]", LBracket, Text ( "a" ), Equals, Text ( "a" ), RBracket, LBracket, Slash, Text ( "a" ), RBracket );

        [TestMethod]
        public void ShouldLexSelfClosingTagWithoutValue ( ) =>
            AssertTokenStream ( "[a/]", LBracket, Text ( "a" ), Slash, RBracket );

        [DataTestMethod]
        [DataRow ( "[[a]" )]
        [DataRow ( "[a[a]" )]
        [DataRow ( "[a=[a]" )]
        public void ShouldNotLex ( String text ) =>
            Assert.ThrowsException<FormatException> ( ( ) =>
            {
                using ( var reader = new StringReader ( text ) )
                {
                    var lexer = new BBLexer ( reader );
                    while ( lexer.NextToken ( ) is BBToken ) ;
                }
            } );
    }
}