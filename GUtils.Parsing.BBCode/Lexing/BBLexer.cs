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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public BBToken? NextToken ( )
        {
            var sequence = new StringBuilder();
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

                case -1:
                    return null;

                default:
                {
                    while ( this.Reader.Peek ( ) != -1
                            && this.Reader.Peek ( ) != '['
                            && ( !this.InsideTag || ( this.Reader.Peek ( ) != '=' && this.Reader.Peek ( ) != ']' ) ) )
                    {
                        if ( this.Reader.Peek ( ) == '\\' )
                            this.Reader.Read ( );

                        sequence.Append ( ( Char ) this.Reader.Read ( ) );
                    }
                    return new BBToken ( BBTokenType.Text, sequence.ToString ( ) );
                }
            }
        }

        public static IEnumerable<BBToken> Lex ( TextReader reader )
        {
            var inTag = false;
            var inValue = false;
            var buffer = new StringBuilder ( );

            while ( reader.Peek ( ) != -1 )
            {
                var peeked = reader.Peek ( );
                if ( inTag )
                {
                    switch ( peeked )
                    {
                        case '[':
                            throw new FormatException ( "Unexpected '[' inside tag." );

                        case ']':
                            inTag = false;
                            reader.Read ( );
                            yield return new BBToken ( BBTokenType.RBracket, "]" );
                            break;

                        case '/':
                            reader.Read ( );
                            yield return new BBToken ( BBTokenType.Slash, "/" );
                            break;


                    }
                }
                switch ( reader.Peek ( ) )
                {
                    case '[':
                        if ( inTag )
                            throw new FormatException ( $"Unexpected '[' inside tag." );
                        inTag = true;
                        reader.Read ( );
                        yield return new BBToken ( BBTokenType.LBracket, "[" );
                        break;

                    case ']':
                        if ( !inTag )
                            goto default;
                        inTag = false;
                        reader.Read ( );
                        yield return new BBToken ( BBTokenType.RBracket, "]" );
                        break;

                    case '/':
                        if ( !inTag )
                            goto default;
                        reader.Read ( );
                        yield return new BBToken ( BBTokenType.Slash, "/" );
                        break;

                    case '=':
                        if ( !inTag )
                            goto default;
                        reader.Read ( );
                        yield return new BBToken ( BBTokenType.Equals, "=" );
                        break;

                    default:
                    {
                        while ( reader.Peek ( ) != -1
                                && reader.Peek ( ) != '['
                                && ( !inTag || ( reader.Peek ( ) != '=' && reader.Peek ( ) != ']' ) ) )
                        {
                            if ( reader.Peek ( ) == '\\' )
                                reader.Read ( );

                            buffer.Append ( ( Char ) reader.Read ( ) );
                        }
                        yield return new BBToken ( BBTokenType.Text, flushBuffer ( ) );
                        break;
                    }
                }
            }

            String flushBuffer ( )
            {
                try
                {
                    return buffer.ToString ( );
                }
                finally
                {
                    buffer.Clear ( );
                }
            }
        }
    }
}