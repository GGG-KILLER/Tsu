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
using GUtils.Parsing.BBCode.Lexing;
using GUtils.Parsing.BBCode.Tree;
using GUtils.Pooling;

namespace GUtils.Parsing.BBCode
{
    /// <summary>
    /// The BBCode parser
    /// </summary>
    /// <remarks>This parser DOES NOT SUPPORT BBCode lists.</remarks>
    public class BBParser : IDisposable
    {
        private readonly StringReader Reader;
        private readonly BBLexer Lexer;
        private readonly Stack<BBTagNode> NodeStack;
        private BBNode Parsed;

        /// <summary>
        /// Initializes a new BBCode parser
        /// </summary>
        /// <param name="code"></param>
        public BBParser ( String code )
        {
            this.Reader = new StringReader ( code );
            this.Lexer = new BBLexer ( this.Reader );
            this.NodeStack = new Stack<BBTagNode> ( );
        }

        private void ParseClosingTag ( )
        {
            BBToken name = this.Lexer.NextToken ( );
            if ( name.Type != BBTokenType.Text )
                throw new FormatException ( "Expected tag name after the slash." );

            var topName = this.NodeStack.Peek ( ).Name;
            if ( topName != name.Value )
                throw new FormatException ( $"Closing tag for '{name.Value}' found but last opened tag was a '{topName}' tag." );
            this.NodeStack.Pop ( );

            if ( this.Lexer.NextToken ( ).Type != BBTokenType.RBracket )
                throw new FormatException ( $"Unfinished closing tag '[/{name.Value}'." );
        }

        private void ParseOpeningTag ( BBToken nameToken )
        {
            String name = nameToken.Value,
                value = null;
            if ( nameToken.Type != BBTokenType.Text )
                throw new FormatException ( $"Expected tag name but got {nameToken.Type}." );

            BBToken rbracket = this.Lexer.NextToken ( );
            if ( rbracket.Type == BBTokenType.Equals )
            {
                BBToken valueToken = this.Lexer.NextToken ( );
                if ( valueToken.Type != BBTokenType.Text )
                    throw new FormatException ( "Value must come after the equals sign." );
                value = valueToken.Value;

                rbracket = this.Lexer.NextToken ( );
            }
            if ( rbracket.Type != BBTokenType.RBracket )
                throw new FormatException ( $"Unfinished tag '{name}'." );

            var node = new BBTagNode ( name, value );
            this.NodeStack.Peek ( ).AddChild ( node );
            this.NodeStack.Push ( node );
        }

        /// <summary>
        /// Parses a string containing BBCode.
        /// </summary>
        /// <returns>A new node as the root with all the parsed contents as children</returns>
        /// <exception cref="FormatException">Thrown when invalid BBCode is fed to the parser</exception>
        public BBNode Parse ( )
        {
            if ( this.Parsed == null )
            {
                this.NodeStack.Push ( new BBTagNode ( "root", null ) );
                while ( this.Reader.Peek ( ) != -1 )
                {
                    BBToken token = this.Lexer.NextToken ( );
                    switch ( token.Type )
                    {
                        case BBTokenType.Text:
                            this.NodeStack
                                .Peek ( )
                                .AddChild ( new BBTextNode ( token.Value ) );
                            break;

                        case BBTokenType.LBracket:
                            token = this.Lexer.NextToken ( );
                            if ( token.Type == BBTokenType.Slash )
                                this.ParseClosingTag ( );
                            else
                                this.ParseOpeningTag ( token );
                            break;

                        default:
                            throw new InvalidOperationException ( "This code broke the parser. Please report it." );
                    }
                }

                if ( this.NodeStack.Count > 1 )
                    throw new FormatException ( $"Unclosed tag '{this.NodeStack.Peek ( ).Name}'." );
                this.Parsed = this.NodeStack.Pop ( );
            }

            return this.Parsed;
        }

        /// <summary>
        /// Parses a string containing BBCode
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static BBNode Parse ( String input )
        {
            using ( var parser = new BBParser ( input ) )
                return parser.Parse ( );
        }

        /// <summary>
        /// Escapes a string so that everything is parsed as a single text node
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String Escape ( String input ) =>
            StringBuilderPool.Shared.WithRentedItem ( b =>
            {
                for ( var i = 0; i < input.Length; i++ )
                {
                    var c = input[i];
                    switch ( c )
                    {
                        case '[':
                        case '\\':
                        case '=':
                        case '/':
                        case ']':
                            b.Append ( '\\' );
                            goto default;

                        default:
                            b.Append ( c );
                            break;
                    }
                }

                return b.ToString ( );
            } );

        #region IDisposable Support

        private Boolean disposedValue; // To detect redundant calls

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose ( Boolean disposing )
        {
            if ( !this.disposedValue )
            {
                if ( disposing )
                {
                    this.Reader.Dispose ( );
                }

                this.Parsed = null;
                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizes this parser
        /// </summary>
        ~BBParser ( )
        {
            this.Dispose ( false );
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public void Dispose ( )
        {
            this.Dispose ( true );
            GC.SuppressFinalize ( this );
        }

        #endregion IDisposable Support
    }
}
