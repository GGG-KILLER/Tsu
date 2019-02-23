using System;
using System.IO;
using GUtils.Pooling;

namespace GUtils.Parsing.BBCode.Lexing
{
    internal class BBLexer
    {
        private readonly TextReader Reader;
        private Boolean InsideTag;

        public BBLexer ( TextReader reader )
        {
            this.Reader = reader;
        }

        public BBToken NextToken ( )
        {
            switch ( this.Reader.Peek ( ) )
            {
                case '[':
                    if ( this.InsideTag )
                        throw new FormatException ( $"Unexpected '[' inside tag." );
                    this.InsideTag = true;
                    this.Reader.Read ( );
                    return new BBToken ( BBTokenType.LBracket, "[" );

                case ']':
                    if ( !this.InsideTag )
                        goto default;
                    this.InsideTag = false;
                    this.Reader.Read ( );
                    return new BBToken ( BBTokenType.RBracket, "]" );

                case '/':
                    if ( !this.InsideTag )
                        goto default;
                    this.Reader.Read ( );
                    return new BBToken ( BBTokenType.Slash, "/" );

                case '=':
                    if ( !this.InsideTag )
                        goto default;
                    this.Reader.Read ( );
                    return new BBToken ( BBTokenType.Equals, "=" );

                default:
                    return new BBToken ( BBTokenType.Text, StringBuilderPool.Shared.WithRentedItem ( sequence =>
                    {
                        while ( ( this.InsideTag ? this.Reader.Peek ( ) != '=' && this.Reader.Peek ( ) != ']' : this.Reader.Peek ( ) != '[' ) && this.Reader.Peek ( ) != -1 )
                        {
                            if ( this.Reader.Peek ( ) == '\\' )
                                this.Reader.Read ( );
                            sequence.Append ( ( Char ) this.Reader.Read ( ) );
                        }
                        return sequence.ToString ( );
                    } ) );
            }
        }
    }
}
