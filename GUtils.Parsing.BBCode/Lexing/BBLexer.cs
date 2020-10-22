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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GUtils.Parsing.BBCode.Lexing
{
    internal static class BBLexer
    {
        public static IEnumerator<BBToken> Lex ( TextReader reader )
        {
            var inTag = false;
            var inValue = false;
            var buffer = new StringBuilder ( );

            while ( reader.Peek ( ) != -1 )
            {
                var peek = reader.Peek ( );

                switch ( peek )
                {
                    case '[':
                    {
                        if ( inTag )
                        {
                            throw new FormatException ( "Unexpected '[' inside tag." );
                        }

                        if ( buffer.Length > 0 )
                            yield return new BBToken ( readAndFlushBuffer ( ) );

                        inTag = true;
                        reader.Read ( );
                        yield return new BBToken ( BBTokenType.LBracket );

                        if ( reader.Peek ( ) == '/' )
                        {
                            reader.Read ( );
                            yield return new BBToken ( BBTokenType.Slash );
                        }
                        break;
                    }

                    case ']':
                    {
                        if ( !inTag )
                        {
                            appendRead ( );
                        }
                        else
                        {
                            if ( buffer.Length > 0 )
                                yield return new BBToken ( readAndFlushBuffer ( ) );

                            inTag = false;
                            inValue = false;
                            reader.Read ( );
                            yield return new BBToken ( BBTokenType.RBracket );
                        }
                        break;
                    }

                    case '/':
                    {
                        if ( !inTag )
                        {
                            appendRead ( );
                        }
                        else
                        {
                            reader.Read ( );

                            if ( reader.Peek ( ) == ']' )
                            {
                                if ( buffer.Length > 0 )
                                    yield return new BBToken ( readAndFlushBuffer ( ) );

                                yield return new BBToken ( BBTokenType.Slash );
                            }
                            else
                            {
                                buffer.Append ( '/' );
                            }
                        }

                        break;
                    }

                    case '=':
                    {
                        if ( !inTag || inValue )
                        {
                            appendRead ( );
                        }
                        else
                        {
                            if ( buffer.Length > 0 )
                                yield return new BBToken ( readAndFlushBuffer ( ) );

                            inValue = true;
                            reader.Read ( );
                            yield return new BBToken ( BBTokenType.Equals );
                        }
                        break;
                    }

                    case '\\':
                    {
                        reader.Read ( );
                        appendRead ( );
                        break;
                    }

                    default:
                    {
                        appendRead ( );
                        break;
                    }
                }
            }

            if ( buffer.Length > 0 )
                yield return new BBToken ( readAndFlushBuffer ( ) );

            String readAndFlushBuffer ( )
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

            void appendRead ( ) => buffer.Append ( ( Char ) reader.Read ( ) );
        }
    }
}