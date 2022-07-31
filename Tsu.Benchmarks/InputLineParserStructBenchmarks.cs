﻿// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Tsu.CLI.Commands.Errors;

#if !NETFRAMEWORK && !NETCOREAPP2_1
#pragma warning disable IDE0057 // Use range operator
#endif

namespace Tsu.Benchmarks
{
    [SimpleJob(RuntimeMoniker.Net48)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31, baseline: true)]
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser(exportHtml: true, exportCombinedDisassemblyReport: true, exportDiff: true)]
    public class InputLineParserStructBenchmarks
    {
        internal class InputLineParserClass
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool IsInRange(char start, char value, char end) =>
                (uint) (value - start) <= (end - start);

            private readonly string _input;
            private int _offset;

            private InputLineParserClass(string input)
            {
                _input = input;
                _offset = 0;
            }

            private string ParseQuotedString(char separator)
            {
                var start = _offset;
                _offset++;

                var builder = new StringBuilder();

                while (_offset < _input.Length && _input[_offset] != separator)
                    builder.Append(ParseCharacter());

                if (_offset == _input.Length || _input[_offset] != separator)
                    throw new InputLineParseException("Unfinished quoted string literal.", start);

                // skip '<separator>'
                _offset++;

                return builder.ToString();
            }

            private char ParseCharacter()
            {
                if (_input[_offset] == '\\')
                {
                    _offset++;
                    if (_offset == _input.Length)
                        throw new InputLineParseException("Unfinished escape.", _offset - 1);

                    switch (_input[_offset++])
                    {
                        case 'a':
                            return '\a';

                        case 'b':
                        {
                            if (_input[_offset] != '0' && _input[_offset] != '1')
                                return '\b';

                            var idx = _offset;
                            while (_input[idx] == '0' || _input[idx] == '1')
                                idx++;

                            var num = _input.Substring(_offset, idx - _offset);
                            _offset = idx;
                            return (char) Convert.ToUInt32(num, 2);
                        }

                        case 'f':
                            return '\f';

                        case 'n':
                            return '\n';

                        case 'o':
                        {
                            var idx = _offset;
                            while (IsInRange('0', _input[idx], '8'))
                                idx++;
                            if (_offset == idx)
                                throw new InputLineParseException("Invalid octal escape.", _offset - 2);

                            var num = _input.Substring(_offset, idx - _offset);
                            _offset = idx;
                            return (char) Convert.ToUInt32(num, 8);
                        }

                        case 'r':
                            return '\r';

                        case 't':
                            return '\t';

                        case 'v':
                            return '\v';

                        /*
                         * hexadecimal_escape_sequence
                         *     : '\\x' hex_digit+
                         */
                        case 'x':
                        {
                            var idx = _offset;
                            while (idx < _input.Length
                                    && (IsInRange('0', _input[idx], '9')
                                         || IsInRange('a', _input[idx], 'f')
                                         || IsInRange('A', _input[idx], 'F')))
                            {
                                idx++;
                            }

                            if (_offset == idx)
                                throw new InputLineParseException("Invalid hexadecimal escape.", _offset - 2);

                            var num = _input.Substring(_offset, idx - _offset);
                            _offset = idx;
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
                            _offset--;
                            var idx = _offset;
                            while (IsInRange('0', _input[idx], '9'))
                                idx++;
                            if (_offset == idx)
                                throw new InputLineParseException("Invalid decimal escape.", _offset - 2);

                            var num = _input.Substring(_offset, idx - _offset);
                            _offset = idx;
                            return (char) Convert.ToUInt32(num, 10);
                        }

                        default:
                            throw new InputLineParseException("Invalid character escape", _offset - 1);
                    }
                }
                else if (_offset < _input.Length)
                {
                    return _input[_offset++];
                }
                else
                {
                    throw new InputLineParseException("Expected char but got EOF", _offset);
                }
            }

            private string ParseSpaceSeparatedSection()
            {
                var builder = new StringBuilder();

                while (_offset < _input.Length
                && !char.IsWhiteSpace(_input[_offset]))
                {
                    builder.Append(ParseCharacter());
                }

                return builder.ToString();
            }

            private void ConsumeWhitespaces()
            {
                while (_offset < _input.Length
                        && char.IsWhiteSpace(_input[_offset]))
                {
                    _offset++;
                }
            }

            private IEnumerable<string> Parse()
            {
                var parts = new List<string>();

                while (_offset < _input.Length)
                {
                    switch (_input[_offset])
                    {
                        // Skip all whitespaces outside of quotes
                        case char ch when char.IsWhiteSpace(ch):
                            ConsumeWhitespaces();
                            break;

                        // Parse quoted strings
                        case char ch when ch == '\'' || ch == '"':
                            parts.Add(ParseQuotedString(ch));
                            break;

                        // (raw)Rest """operator""" (r:)
                        case 'r':

                            // Raw rest
                            if (_input[_offset + 1] == 'r'
                                && _input[_offset + 2] == ':')
                            {
                                // Move from 'r' while skipping 'r' and ':'
                                _offset += 3;

                                parts.Add(_input.Substring(_offset));
                                _offset = _input.Length;
                                break;
                            }
                            else if (_input[_offset + 1] == ':')
                            {
                                // Move from 'r' while skipping ':'
                                _offset += 2;

                                var builder = new StringBuilder();
                                while (_offset < _input.Length)
                                    builder.Append(ParseCharacter());
                                _offset = _input.Length;

                                // We do not return non-explicit empty strings under any circumstances
                                if (builder.Length > 0)
                                    parts.Add(builder.ToString());
                                break;
                            }
                            else
                            {
                                goto default;
                            }

                        // Normal space-separated string
                        default:
                            parts.Add(ParseSpaceSeparatedSection());
                            break;
                    }
                }

                return parts.ToArray();
            }

            public static IEnumerable<string> Parse(string line) =>
                new InputLineParserClass(line).Parse();
        }

        internal struct InputLineParserStruct
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool IsInRange(char start, char value, char end) =>
                (uint) (value - start) <= (end - start);

            private readonly string _input;
            private int _offset;

            private InputLineParserStruct(string input)
            {
                _input = input;
                _offset = 0;
            }

            private string ParseQuotedString(char separator)
            {
                var start = _offset;
                _offset++;

                var builder = new StringBuilder();

                while (_offset < _input.Length && _input[_offset] != separator)
                    builder.Append(ParseCharacter());

                if (_offset == _input.Length || _input[_offset] != separator)
                    throw new InputLineParseException("Unfinished quoted string literal.", start);

                // skip '<separator>'
                _offset++;

                return builder.ToString();
            }

            private char ParseCharacter()
            {
                if (_input[_offset] == '\\')
                {
                    _offset++;
                    if (_offset == _input.Length)
                        throw new InputLineParseException("Unfinished escape.", _offset - 1);

                    switch (_input[_offset++])
                    {
                        case 'a':
                            return '\a';

                        case 'b':
                        {
                            if (_input[_offset] != '0' && _input[_offset] != '1')
                                return '\b';

                            var idx = _offset;
                            while (_input[idx] == '0' || _input[idx] == '1')
                                idx++;

                            var num = _input.Substring(_offset, idx - _offset);
                            _offset = idx;
                            return (char) Convert.ToUInt32(num, 2);
                        }

                        case 'f':
                            return '\f';

                        case 'n':
                            return '\n';

                        case 'o':
                        {
                            var idx = _offset;
                            while (IsInRange('0', _input[idx], '8'))
                                idx++;
                            if (_offset == idx)
                                throw new InputLineParseException("Invalid octal escape.", _offset - 2);

                            var num = _input.Substring(_offset, idx - _offset);
                            _offset = idx;
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
                            var idx = _offset;
                            while (idx < _input.Length
                                    && (IsInRange('0', _input[idx], '9')
                                         || IsInRange('a', _input[idx], 'f')
                                         || IsInRange('A', _input[idx], 'F')))
                            {
                                idx++;
                            }

                            if (_offset == idx)
                                throw new InputLineParseException("Invalid hexadecimal escape.", _offset - 2);

                            var num = _input.Substring(_offset, idx - _offset);
                            _offset = idx;
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
                            _offset--;
                            var idx = _offset;
                            while (IsInRange('0', _input[idx], '9'))
                                idx++;
                            if (_offset == idx)
                                throw new InputLineParseException("Invalid decimal escape.", _offset - 2);

                            var num = _input.Substring(_offset, idx - _offset);
                            _offset = idx;
                            return (char) Convert.ToUInt32(num, 10);
                        }

                        default:
                            throw new InputLineParseException("Invalid character escape", _offset - 1);
                    }
                }
                else if (_offset < _input.Length)
                {
                    return _input[_offset++];
                }
                else
                {
                    throw new InputLineParseException("Expected char but got EOF", _offset);
                }
            }

            private string ParseSpaceSeparatedSection()
            {
                var builder = new StringBuilder();

                while (_offset < _input.Length
                && !char.IsWhiteSpace(_input[_offset]))
                {
                    builder.Append(ParseCharacter());
                }

                return builder.ToString();
            }

            private void ConsumeWhitespaces()
            {
                while (_offset < _input.Length
                        && char.IsWhiteSpace(_input[_offset]))
                {
                    _offset++;
                }
            }

            private IEnumerable<string> Parse()
            {
                var parts = new List<string>();

                while (_offset < _input.Length)
                {
                    switch (_input[_offset])
                    {
                        // Skip all whitespaces outside of quotes
                        case char ch when char.IsWhiteSpace(ch):
                            ConsumeWhitespaces();
                            break;

                        // Parse quoted strings
                        case char ch when ch == '\'' || ch == '"':
                            parts.Add(ParseQuotedString(ch));
                            break;

                        // (raw)Rest """operator""" (r:)
                        case 'r':

                            // Raw rest
                            if (_input[_offset + 1] == 'r'
                                && _input[_offset + 2] == ':')
                            {
                                // Move from 'r' while skipping 'r' and ':'
                                _offset += 3;

                                parts.Add(_input.Substring(_offset));
                                _offset = _input.Length;
                                break;
                            }
                            else if (_input[_offset + 1] == ':')
                            {
                                // Move from 'r' while skipping ':'
                                _offset += 2;

                                var builder = new StringBuilder();
                                while (_offset < _input.Length)
                                    builder.Append(ParseCharacter());
                                _offset = _input.Length;

                                // We do not return non-explicit empty strings under any circumstances
                                if (builder.Length > 0)
                                    parts.Add(builder.ToString());
                                break;
                            }
                            else
                            {
                                goto default;
                            }

                        // Normal space-separated string
                        default:
                            parts.Add(ParseSpaceSeparatedSection());
                            break;
                    }
                }

                return parts.ToArray();
            }

            public static IEnumerable<string> Parse(string line) =>
                new InputLineParserStruct(line).Parse();
        }

        [Params(
            "simple input",
            @"'\b1100011\o157\109\6cx' input",
            @"rr:raw rest input",
            @"r:rest \b1100011\o157\109\6cx input")]
        public string Input { get; set; } = null!;

        [Benchmark(Baseline = true)]
        public string[] ClassParser() =>
            InputLineParserClass.Parse(Input).ToArray();

        [Benchmark]
        public string[] StructParser() =>
            InputLineParserStruct.Parse(Input).ToArray();
    }
}