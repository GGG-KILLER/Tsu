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
using System.Runtime.CompilerServices;
using System.Text;
using Tsu.CLI.Commands.Errors;

namespace Tsu.CLI.Commands
{
    /// <summary>
    /// The input line parser.
    /// </summary>
    public class InputLineParser
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsInRange(char start, char value, char end) =>
            (uint) (value - start) <= (end - start);

        private readonly string Input;
        private int Offset;

        private InputLineParser(string input)
        {
            Input = input;
            Offset = 0;
        }

        private string ParseQuotedString(char separator)
        {
            var start = Offset;
            Offset++;

            var builder = new StringBuilder();

            while (Offset < Input.Length && Input[Offset] != separator)
                builder.Append(ParseCharacter());

            if (Offset == Input.Length || Input[Offset] != separator)
                throw new InputLineParseException("Unfinished quoted string literal.", start);

            // skip '<separator>'
            Offset++;

            return builder.ToString();
        }

        private char ParseCharacter()
        {
            if (Input[Offset] == '\\')
            {
                Offset++;
                if (Offset == Input.Length)
                    throw new InputLineParseException("Unfinished escape.", Offset - 1);

                switch (Input[Offset++])
                {
                    case 'a':
                        return '\a';

                    case 'b':
                    {
                        if (Input[Offset] != '0' && Input[Offset] != '1')
                            return '\b';

                        var idx = Offset;
                        while (Input[idx] == '0' || Input[idx] == '1')
                            idx++;

                        var num = Input.Substring(Offset, idx - Offset);
                        Offset = idx;
                        return (char) Convert.ToUInt32(num, 2);
                    }

                    case 'f':
                        return '\f';

                    case 'n':
                        return '\n';

                    case 'o':
                    {
                        var idx = Offset;
                        while (IsInRange('0', Input[idx], '8'))
                            idx++;
                        if (Offset == idx)
                            throw new InputLineParseException("Invalid octal escape.", Offset - 2);

                        var num = Input.Substring(Offset, idx - Offset);
                        Offset = idx;
                        return (char) Convert.ToUInt32(num, 8);
                    }

                    case 'r':
                        return '\r';

                    case 't':
                        return '\t';

                    case 'v':
                        return '\v';

                    case 'x':
                    {
                        var idx = Offset;
                        while (idx < Input.Length
                               && (IsInRange('0', Input[idx], '9')
                                   || IsInRange('a', Input[idx], 'f')
                                   || IsInRange('A', Input[idx], 'F')))
                        {
                            idx++;
                        }

                        if (Offset == idx)
                            throw new InputLineParseException("Invalid hexadecimal escape.", Offset - 2);

                        var num = Input.Substring(Offset, idx - Offset);
                        Offset = idx;
                        return (char) Convert.ToUInt32(num, 16);
                    }

                    case ' ':
                        return ' ';

                    case '"':
                        return '"';

                    case '\'':
                        return '\'';

                    case '\\':
                        return '\\';

                    case char ch when IsInRange('0', ch, '9'):
                    {
                        // We ended up consuming one of the digits on this one
                        Offset--;
                        var idx = Offset;
                        while (IsInRange('0', Input[idx], '9'))
                            idx++;
                        if (Offset == idx)
                            throw new InputLineParseException("Invalid decimal escape.", Offset - 2);

                        var num = Input.Substring(Offset, idx - Offset);
                        Offset = idx;
                        return (char) Convert.ToUInt32(num, 10);
                    }

                    default:
                        throw new InputLineParseException("Invalid character escape", Offset - 1);
                }
            }
            else if (Offset < Input.Length)
            {
                return Input[Offset++];
            }
            else
            {
                throw new InputLineParseException("Expected char but got EOF", Offset);
            }
        }

        private string ParseSpaceSeparatedSection()
        {
            var builder = new StringBuilder();

            while (Offset < Input.Length
            && !char.IsWhiteSpace(Input[Offset]))
            {
                builder.Append(ParseCharacter());
            }

            return builder.ToString();
        }

        private void ConsumeWhitespaces()
        {
            while (Offset < Input.Length
                   && char.IsWhiteSpace(Input[Offset]))
            {
                Offset++;
            }
        }

        private IEnumerable<string> Parse()
        {
            while (Offset < Input.Length)
            {
                switch (Input[Offset])
                {
                    // Skip all whitespaces outside of quotes
                    case char ch when char.IsWhiteSpace(ch):
                        ConsumeWhitespaces();
                        break;

                    // Parse quoted strings
                    case char ch when ch == '\'' || ch == '"':
                        yield return ParseQuotedString(ch);
                        break;

                    // (raw)Rest """operator""" (r:)
                    case 'r':

                        // Raw rest
                        if (Input[Offset + 1] == 'r'
                            && Input[Offset + 2] == ':')
                        {
                            // Move from 'r' while skipping 'r' and ':'
                            Offset += 3;

                            yield return Input.Substring(Offset);
                            Offset = Input.Length;
                            break;
                        }
                        else if (Input[Offset + 1] == ':')
                        {
                            // Move from 'r' while skipping ':'
                            Offset += 2;

                            var builder = new StringBuilder();
                            while (Offset < Input.Length)
                                builder.Append(ParseCharacter());
                            Offset = Input.Length;

                            // We do not return non-explicit empty strings under any circumstances
                            if (builder.Length > 0)
                                yield return builder.ToString();
                            break;
                        }
                        else
                        {
                            goto default;
                        }

                    // Normal space-separated string
                    default:
                        yield return ParseSpaceSeparatedSection();
                        break;
                }
            }
        }

        /// <summary>
        /// Parses an input line.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static IEnumerable<string> Parse(string line) =>
            new InputLineParser(line).Parse();
    }
}