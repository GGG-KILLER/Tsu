using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using GUtils.CLI.Commands.Errors;

#if !NETFRAMEWORK && !NETCOREAPP2_1
#pragma warning disable IDE0057 // Use range operator
#endif

namespace GUtils.Benchmarks
{
    [SimpleJob ( RuntimeMoniker.Net48 )]
    [SimpleJob ( RuntimeMoniker.NetCoreApp31, baseline: true )]
    [SimpleJob ( RuntimeMoniker.NetCoreApp50 )]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser ( exportHtml: true, exportCombinedDisassemblyReport: true, exportDiff: true )]
    public class InputLineParserStructBenchmarks
    {
        internal class InputLineParserClass
        {
            [MethodImpl ( MethodImplOptions.AggressiveInlining )]
            private static Boolean IsInRange ( Char start, Char value, Char end ) =>
                ( UInt32 ) ( value - start ) <= ( end - start );

            private readonly String Input;
            private Int32 Offset;

            private InputLineParserClass ( String input )
            {
                this.Input = input;
                this.Offset = 0;
            }

            private String ParseQuotedString ( Char separator )
            {
                var start = this.Offset;
                this.Offset++;

                var builder = new StringBuilder ( );

                while ( this.Offset < this.Input.Length && this.Input[this.Offset] != separator )
                    builder.Append ( this.ParseCharacter ( ) );

                if ( this.Offset == this.Input.Length || this.Input[this.Offset] != separator )
                    throw new InputLineParseException ( "Unfinished quoted string literal.", start );

                // skip '<separator>'
                this.Offset++;

                return builder.ToString ( );
            }

            private Char ParseCharacter ( )
            {
                if ( this.Input[this.Offset] == '\\' )
                {
                    this.Offset++;
                    if ( this.Offset == this.Input.Length )
                        throw new InputLineParseException ( "Unfinished escape.", this.Offset - 1 );

                    switch ( this.Input[this.Offset++] )
                    {
                        case 'a':
                            return '\a';

                        case 'b':
                        {
                            if ( this.Input[this.Offset] != '0' && this.Input[this.Offset] != '1' )
                                return '\b';

                            var idx = this.Offset;
                            while ( this.Input[idx] == '0' || this.Input[idx] == '1' )
                                idx++;

                            var num = this.Input.Substring ( this.Offset, idx - this.Offset );
                            this.Offset = idx;
                            return ( Char ) Convert.ToUInt32 ( num, 2 );
                        }

                        case 'f':
                            return '\f';

                        case 'n':
                            return '\n';

                        case 'o':
                        {
                            var idx = this.Offset;
                            while ( IsInRange ( '0', this.Input[idx], '8' ) )
                                idx++;
                            if ( this.Offset == idx )
                                throw new InputLineParseException ( "Invalid octal escape.", this.Offset - 2 );

                            var num = this.Input.Substring ( this.Offset, idx - this.Offset );
                            this.Offset = idx;
                            return ( Char ) Convert.ToUInt32 ( num, 8 );
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
                            var idx = this.Offset;
                            while ( idx < this.Input.Length
                                    && ( IsInRange ( '0', this.Input[idx], '9' )
                                         || IsInRange ( 'a', this.Input[idx], 'f' )
                                         || IsInRange ( 'A', this.Input[idx], 'F' ) ) )
                            {
                                idx++;
                            }

                            if ( this.Offset == idx )
                                throw new InputLineParseException ( "Invalid hexadecimal escape.", this.Offset - 2 );

                            var num = this.Input.Substring ( this.Offset, idx - this.Offset );
                            this.Offset = idx;
                            return ( Char ) Convert.ToUInt32 ( num, 16 );
                        }

                        case ' ':
                            return ' ';

                        case '"':
                            return '"';

                        case '\'':
                            return '\'';

                        case '\\':
                            return '\\';

                        case Char ch when IsInRange ( '0', ch, '9' ):
                        {
                            // We ended up consuming one of the digits on this one
                            this.Offset--;
                            var idx = this.Offset;
                            while ( IsInRange ( '0', this.Input[idx], '9' ) )
                                idx++;
                            if ( this.Offset == idx )
                                throw new InputLineParseException ( "Invalid decimal escape.", this.Offset - 2 );

                            var num = this.Input.Substring ( this.Offset, idx - this.Offset );
                            this.Offset = idx;
                            return ( Char ) Convert.ToUInt32 ( num, 10 );
                        }

                        default:
                            throw new InputLineParseException ( "Invalid character escape", this.Offset - 1 );
                    }
                }
                else if ( this.Offset < this.Input.Length )
                {
                    return this.Input[this.Offset++];
                }
                else
                {
                    throw new InputLineParseException ( "Expected char but got EOF", this.Offset );
                }
            }

            private String ParseSpaceSeparatedSection ( )
            {
                var builder = new StringBuilder ( );

                while ( this.Offset < this.Input.Length
                && !Char.IsWhiteSpace ( this.Input[this.Offset] ) )
                {
                    builder.Append ( this.ParseCharacter ( ) );
                }

                return builder.ToString ( );
            }

            private void ConsumeWhitespaces ( )
            {
                while ( this.Offset < this.Input.Length
                        && Char.IsWhiteSpace ( this.Input[this.Offset] ) )
                {
                    this.Offset++;
                }
            }

            private IEnumerable<String> Parse ( )
            {
                var parts = new List<String> ( );

                while ( this.Offset < this.Input.Length )
                {
                    switch ( this.Input[this.Offset] )
                    {
                        // Skip all whitespaces outside of quotes
                        case Char ch when Char.IsWhiteSpace ( ch ):
                            this.ConsumeWhitespaces ( );
                            break;

                        // Parse quoted strings
                        case Char ch when ch == '\'' || ch == '"':
                            parts.Add ( this.ParseQuotedString ( ch ) );
                            break;

                        // (raw)Rest """operator""" (r:)
                        case 'r':

                            // Raw rest
                            if ( this.Input[this.Offset + 1] == 'r'
                                && this.Input[this.Offset + 2] == ':' )
                            {
                                // Move from 'r' while skipping 'r' and ':'
                                this.Offset += 3;

                                parts.Add ( this.Input.Substring ( this.Offset ) );
                                this.Offset = this.Input.Length;
                                break;
                            }
                            else if ( this.Input[this.Offset + 1] == ':' )
                            {
                                // Move from 'r' while skipping ':'
                                this.Offset += 2;

                                var builder = new StringBuilder ( );
                                while ( this.Offset < this.Input.Length )
                                    builder.Append ( this.ParseCharacter ( ) );
                                this.Offset = this.Input.Length;

                                // We do not return non-explicit empty strings under any circumstances
                                if ( builder.Length > 0 )
                                    parts.Add ( builder.ToString ( ) );
                                break;
                            }
                            else
                            {
                                goto default;
                            }

                        // Normal space-separated string
                        default:
                            parts.Add ( this.ParseSpaceSeparatedSection ( ) );
                            break;
                    }
                }

                return parts.ToArray ( );
            }

            public static IEnumerable<String> Parse ( String line ) =>
                new InputLineParserClass ( line ).Parse ( );
        }

        internal struct InputLineParserStruct
        {
            [MethodImpl ( MethodImplOptions.AggressiveInlining )]
            private static Boolean IsInRange ( Char start, Char value, Char end ) =>
                ( UInt32 ) ( value - start ) <= ( end - start );

            private readonly String Input;
            private Int32 Offset;

            private InputLineParserStruct ( String input )
            {
                this.Input = input;
                this.Offset = 0;
            }

            private String ParseQuotedString ( Char separator )
            {
                var start = this.Offset;
                this.Offset++;

                var builder = new StringBuilder ( );

                while ( this.Offset < this.Input.Length && this.Input[this.Offset] != separator )
                    builder.Append ( this.ParseCharacter ( ) );

                if ( this.Offset == this.Input.Length || this.Input[this.Offset] != separator )
                    throw new InputLineParseException ( "Unfinished quoted string literal.", start );

                // skip '<separator>'
                this.Offset++;

                return builder.ToString ( );
            }

            private Char ParseCharacter ( )
            {
                if ( this.Input[this.Offset] == '\\' )
                {
                    this.Offset++;
                    if ( this.Offset == this.Input.Length )
                        throw new InputLineParseException ( "Unfinished escape.", this.Offset - 1 );

                    switch ( this.Input[this.Offset++] )
                    {
                        case 'a':
                            return '\a';

                        case 'b':
                        {
                            if ( this.Input[this.Offset] != '0' && this.Input[this.Offset] != '1' )
                                return '\b';

                            var idx = this.Offset;
                            while ( this.Input[idx] == '0' || this.Input[idx] == '1' )
                                idx++;

                            var num = this.Input.Substring ( this.Offset, idx - this.Offset );
                            this.Offset = idx;
                            return ( Char ) Convert.ToUInt32 ( num, 2 );
                        }

                        case 'f':
                            return '\f';

                        case 'n':
                            return '\n';

                        case 'o':
                        {
                            var idx = this.Offset;
                            while ( IsInRange ( '0', this.Input[idx], '8' ) )
                                idx++;
                            if ( this.Offset == idx )
                                throw new InputLineParseException ( "Invalid octal escape.", this.Offset - 2 );

                            var num = this.Input.Substring ( this.Offset, idx - this.Offset );
                            this.Offset = idx;
                            return ( Char ) Convert.ToUInt32 ( num, 8 );
                        }

                        case 'r':
                            return '\r';

                        case 't':
                            return '\t';

                        case 'v':
                            return '\v';

                        case 'x':
                        {
                            var idx = this.Offset;
                            while ( idx < this.Input.Length
                                    && ( IsInRange ( '0', this.Input[idx], '9' )
                                         || IsInRange ( 'a', this.Input[idx], 'f' )
                                         || IsInRange ( 'A', this.Input[idx], 'F' ) ) )
                            {
                                idx++;
                            }

                            if ( this.Offset == idx )
                                throw new InputLineParseException ( "Invalid hexadecimal escape.", this.Offset - 2 );

                            var num = this.Input.Substring ( this.Offset, idx - this.Offset );
                            this.Offset = idx;
                            return ( Char ) Convert.ToUInt32 ( num, 16 );
                        }

                        case ' ':
                            return ' ';

                        case '"':
                            return '"';

                        case '\'':
                            return '\'';

                        case '\\':
                            return '\\';

                        case Char ch when IsInRange ( '0', ch, '9' ):
                        {
                            // We ended up consuming one of the digits on this one
                            this.Offset--;
                            var idx = this.Offset;
                            while ( IsInRange ( '0', this.Input[idx], '9' ) )
                                idx++;
                            if ( this.Offset == idx )
                                throw new InputLineParseException ( "Invalid decimal escape.", this.Offset - 2 );

                            var num = this.Input.Substring ( this.Offset, idx - this.Offset );
                            this.Offset = idx;
                            return ( Char ) Convert.ToUInt32 ( num, 10 );
                        }

                        default:
                            throw new InputLineParseException ( "Invalid character escape", this.Offset - 1 );
                    }
                }
                else if ( this.Offset < this.Input.Length )
                {
                    return this.Input[this.Offset++];
                }
                else
                {
                    throw new InputLineParseException ( "Expected char but got EOF", this.Offset );
                }
            }

            private String ParseSpaceSeparatedSection ( )
            {
                var builder = new StringBuilder ( );

                while ( this.Offset < this.Input.Length
                && !Char.IsWhiteSpace ( this.Input[this.Offset] ) )
                {
                    builder.Append ( this.ParseCharacter ( ) );
                }

                return builder.ToString ( );
            }

            private void ConsumeWhitespaces ( )
            {
                while ( this.Offset < this.Input.Length
                        && Char.IsWhiteSpace ( this.Input[this.Offset] ) )
                {
                    this.Offset++;
                }
            }

            private IEnumerable<String> Parse ( )
            {
                var parts = new List<String> ( );

                while ( this.Offset < this.Input.Length )
                {
                    switch ( this.Input[this.Offset] )
                    {
                        // Skip all whitespaces outside of quotes
                        case Char ch when Char.IsWhiteSpace ( ch ):
                            this.ConsumeWhitespaces ( );
                            break;

                        // Parse quoted strings
                        case Char ch when ch == '\'' || ch == '"':
                            parts.Add ( this.ParseQuotedString ( ch ) );
                            break;

                        // (raw)Rest """operator""" (r:)
                        case 'r':

                            // Raw rest
                            if ( this.Input[this.Offset + 1] == 'r'
                                && this.Input[this.Offset + 2] == ':' )
                            {
                                // Move from 'r' while skipping 'r' and ':'
                                this.Offset += 3;

                                parts.Add ( this.Input.Substring ( this.Offset ) );
                                this.Offset = this.Input.Length;
                                break;
                            }
                            else if ( this.Input[this.Offset + 1] == ':' )
                            {
                                // Move from 'r' while skipping ':'
                                this.Offset += 2;

                                var builder = new StringBuilder ( );
                                while ( this.Offset < this.Input.Length )
                                    builder.Append ( this.ParseCharacter ( ) );
                                this.Offset = this.Input.Length;

                                // We do not return non-explicit empty strings under any circumstances
                                if ( builder.Length > 0 )
                                    parts.Add ( builder.ToString ( ) );
                                break;
                            }
                            else
                            {
                                goto default;
                            }

                        // Normal space-separated string
                        default:
                            parts.Add ( this.ParseSpaceSeparatedSection ( ) );
                            break;
                    }
                }

                return parts.ToArray ( );
            }

            public static IEnumerable<String> Parse ( String line ) =>
                new InputLineParserStruct ( line ).Parse ( );
        }

        [Params (
            "simple input",
            @"'\b1100011\o157\109\6cx' input",
            @"rr:raw rest input",
            @"r:rest \b1100011\o157\109\6cx input" )]
        public String Input { get; set; }

        [Benchmark ( Baseline = true )]
        public String[] ClassParser ( ) =>
            InputLineParserClass.Parse ( this.Input ).ToArray ( );

        [Benchmark]
        public String[] StructParser ( ) =>
            InputLineParserStruct.Parse ( this.Input ).ToArray ( );
    }
}