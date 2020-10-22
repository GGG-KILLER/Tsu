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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using GUtils.Parsing.BBCode.Lexing;
using GUtils.Parsing.BBCode.Tree;

namespace GUtils.Parsing.BBCode
{
    /// <summary>
    /// The BBCode parser
    /// </summary>
    /// <remarks>This parser DOES NOT SUPPORT BBCode lists.</remarks>
    public class BBParser : IDisposable
    {
        [SuppressMessage ( "Usage", "CA2213:Disposable fields should be disposed", Justification = "Is being disposed on Dispose(bool)." )]
        private readonly StringReader Reader;
        private readonly IEnumerator<BBToken> Lexer;
        private readonly Stack<BBTagNode> NodeStack;
        private BBNode[]? Parsed;

        /// <summary>
        /// Initializes a new BBCode parser
        /// </summary>
        /// <param name="code"></param>
        public BBParser ( String code )
        {
            this.Reader = new StringReader ( code );
            this.Lexer = BBLexer.Lex ( this.Reader ).GetEnumerator ( );
            this.NodeStack = new Stack<BBTagNode> ( );
            this.Parsed = null;
        }

        private BBToken? GetNextToken ( )
        {
            if ( this.Lexer.MoveNext ( ) )
            {
                return this.Lexer.Current;
            }

            return null;
        }

        private void ParseClosingTag ( )
        {
            BBToken? name = this.GetNextToken ( );
            if ( name?.Type != BBTokenType.Text )
                throw new FormatException ( "Expected tag name after the slash." );

            var topName = this.NodeStack.Peek ( ).Name;
            if ( topName != name.Value.Value )
                throw new FormatException ( $"Closing tag for '{name.Value}' found but last opened tag was a '{topName}' tag." );
            this.NodeStack.Pop ( );

            if ( this.GetNextToken ( )?.Type != BBTokenType.RBracket )
                throw new FormatException ( $"Unfinished closing tag '[/{name.Value}'." );
        }

        private void ParseOpeningTag ( BBToken nameToken )
        {
            var selfClosing = false;
            var name = nameToken.Value!;
            String? value = null;
            if ( nameToken.Type != BBTokenType.Text )
                throw new FormatException ( $"Expected tag name but got {nameToken.Type}." );

            BBToken? rbracket = this.GetNextToken ( );
            if ( rbracket?.Type == BBTokenType.Equals )
            {
                BBToken? valueToken = this.GetNextToken ( );
                if ( valueToken?.Type != BBTokenType.Text )
                    throw new FormatException ( "Value must come after the equals sign." );
                value = valueToken.Value.Value;

                rbracket = this.GetNextToken ( );
            }
            if ( rbracket?.Type == BBTokenType.Slash )
            {
                selfClosing = true;
                rbracket = this.GetNextToken ( );
            }
            if ( rbracket?.Type != BBTokenType.RBracket )
                throw new FormatException ( $"Unfinished tag '{name}'." );

            var node = new BBTagNode ( selfClosing, name, value );
            this.NodeStack.Peek ( ).AddChild ( node );
            if ( !selfClosing )
            {
                this.NodeStack.Push ( node );
            }
        }

        /// <summary>
        /// Parses a string containing BBCode.
        /// </summary>
        /// <returns>A new node as the root with all the parsed contents as children</returns>
        /// <exception cref="FormatException">Thrown when invalid BBCode is fed to the parser</exception>
        public BBNode[] Parse ( )
        {
            if ( this.Parsed == null )
            {
                this.NodeStack.Push ( new BBTagNode ( false, "root", null ) );
                while ( this.Reader.Peek ( ) != -1 )
                {
                    BBToken? token = this.GetNextToken ( );
                    switch ( token?.Type )
                    {
                        case BBTokenType.Text:
                            this.NodeStack
                                .Peek ( )
                                .AddChild ( new BBTextNode ( token.Value.Value! ) );
                            break;

                        case BBTokenType.LBracket:
                            token = this.GetNextToken ( );
                            if ( token is null )
                                throw new FormatException ( "Unfinished tag opening." );
                            else if ( token.Value.Type == BBTokenType.Slash )
                                this.ParseClosingTag ( );
                            else
                                this.ParseOpeningTag ( token.Value );
                            break;

                        default:
                            throw new InvalidOperationException ( "This code broke the parser. Please report it." );
                    }
                }

                if ( this.NodeStack.Count > 1 )
                    throw new FormatException ( $"Unclosed tag '{this.NodeStack.Peek ( ).Name}'." );
                this.Parsed = this.NodeStack.Pop ( ).Children.ToArray ( );
            }

            return this.Parsed;
        }

        /// <summary>
        /// Parses a string containing BBCode
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static BBNode[] Parse ( String input )
        {
            using var parser = new BBParser ( input );
            return parser.Parse ( );
        }

        /// <summary>
        /// Escapes a string so that everything is parsed as a single text node
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String Escape ( String input )
        {
            if ( input is null )
                throw new ArgumentNullException ( nameof ( input ) );

            var builder = new StringBuilder ( );

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
                        builder.Append ( '\\' );
                        goto default;

                    default:
                        builder.Append ( c );
                        break;
                }
            }

            return builder.ToString ( );
        }

        #region IDisposable Support

        private Boolean disposedValue; // To detect redundant calls

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose ( Boolean disposing )
        {
            if ( !this.disposedValue )
            {
                if ( disposing )
                {
                    this.Reader.Dispose ( );
                    this.Lexer.Dispose ( );
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
        /// <inheritdoc/>
        /// </summary>
        public void Dispose ( )
        {
            this.Dispose ( true );
            GC.SuppressFinalize ( this );
        }

        #endregion IDisposable Support
    }
}