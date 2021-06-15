// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the “Software”), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using Tsu.Parsing.BBCode.Lexing;
using Tsu.Parsing.BBCode.Tree;

namespace Tsu.Parsing.BBCode
{
    /// <summary>
    /// The BBCode parser
    /// </summary>
    /// <remarks>This parser DOES NOT SUPPORT BBCode lists.</remarks>
    public class BBParser : IDisposable
    {
        [SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Is being disposed on Dispose(bool).")]
        private readonly StringReader Reader;
        private readonly IEnumerator<BBToken> Lexer;
        private readonly Stack<BBTagNode> NodeStack;
        private BBNode[]? Parsed;

        /// <summary>
        /// Initializes a new BBCode parser
        /// </summary>
        /// <param name="code"></param>
        public BBParser(string code)
        {
            Reader = new StringReader(code);
            Lexer = BBLexer.Lex(Reader).GetEnumerator();
            NodeStack = new Stack<BBTagNode>();
            Parsed = null;
        }

        private BBToken? GetNextToken()
        {
            if (Lexer.MoveNext())
            {
                return Lexer.Current;
            }

            return null;
        }

        private void ParseClosingTag()
        {
            var name = GetNextToken();
            if (name?.Type != BBTokenType.Text)
                throw new FormatException("Expected tag name after the slash.");

            var topName = NodeStack.Peek().Name;
            if (topName != name.Value.Value)
                throw new FormatException($"Closing tag for '{name.Value}' found but last opened tag was a '{topName}' tag.");
            NodeStack.Pop();

            if (GetNextToken()?.Type != BBTokenType.RBracket)
                throw new FormatException($"Unfinished closing tag '[/{name.Value}'.");
        }

        private void ParseOpeningTag(BBToken nameToken)
        {
            var selfClosing = false;
            var name = nameToken.Value!;
            string? value = null;
            if (nameToken.Type != BBTokenType.Text)
                throw new FormatException($"Expected tag name but got {nameToken.Type}.");

            var rbracket = GetNextToken();
            if (rbracket?.Type == BBTokenType.Equals)
            {
                var valueToken = GetNextToken();
                if (valueToken?.Type != BBTokenType.Text)
                    throw new FormatException("Value must come after the equals sign.");
                value = valueToken.Value.Value;

                rbracket = GetNextToken();
            }
            if (rbracket?.Type == BBTokenType.Slash)
            {
                selfClosing = true;
                rbracket = GetNextToken();
            }
            if (rbracket?.Type != BBTokenType.RBracket)
                throw new FormatException($"Unfinished tag '{name}'.");

            var node = new BBTagNode(selfClosing, name, value);
            NodeStack.Peek().AddChild(node);
            if (!selfClosing)
            {
                NodeStack.Push(node);
            }
        }

        /// <summary>
        /// Parses a string containing BBCode.
        /// </summary>
        /// <returns>A new node as the root with all the parsed contents as children</returns>
        /// <exception cref="FormatException">Thrown when invalid BBCode is fed to the parser</exception>
        public BBNode[] Parse()
        {
            if (Parsed == null)
            {
                NodeStack.Push(new BBTagNode(false, "root", null));
                while (Reader.Peek() != -1)
                {
                    var token = GetNextToken();
                    switch (token?.Type)
                    {
                        case BBTokenType.Text:
                            NodeStack
                                .Peek()
                                .AddChild(new BBTextNode(token.Value.Value!));
                            break;

                        case BBTokenType.LBracket:
                            token = GetNextToken();
                            if (token is null)
                                throw new FormatException("Unfinished tag opening.");
                            else if (token.Value.Type == BBTokenType.Slash)
                                ParseClosingTag();
                            else
                                ParseOpeningTag(token.Value);
                            break;

                        default:
                            throw new InvalidOperationException("This code broke the parser. Please report it.");
                    }
                }

                if (NodeStack.Count > 1)
                    throw new FormatException($"Unclosed tag '{NodeStack.Peek().Name}'.");
                Parsed = NodeStack.Pop().Children.ToArray();
            }

            return Parsed;
        }

        /// <summary>
        /// Parses a string containing BBCode
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static BBNode[] Parse(string input)
        {
            using var parser = new BBParser(input);
            return parser.Parse();
        }

        /// <summary>
        /// Escapes a string so that everything is parsed as a single text node
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Escape(string input)
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));

            var builder = new StringBuilder();

            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                switch (c)
                {
                    case '[':
                    case '\\':
                    case '=':
                    case '/':
                    case ']':
                        builder.Append('\\');
                        goto default;

                    default:
                        builder.Append(c);
                        break;
                }
            }

            return builder.ToString();
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Reader.Dispose();
                    Lexer.Dispose();
                }

                Parsed = null;
                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizes this parser
        /// </summary>
        ~BBParser()
        {
            Dispose(false);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}