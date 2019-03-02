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
