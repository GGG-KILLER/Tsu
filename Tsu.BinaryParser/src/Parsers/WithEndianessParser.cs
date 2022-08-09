// Copyright © 2021 GGG KILLER <gggkiller2@gmail.com>
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
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Tsu.BinaryParser.Parsers;

namespace Tsu.BinaryParser.Parsers
{
    /// <summary>
    /// A meta-parser that forcefully sets the endianess for a given parser it wraps.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class WithEndianessParser<T> : IBinaryParser<T>
    {
        private readonly IBinaryParser<T> _wrappedParser;
        private readonly Endianess _endianess;

        /// <summary>
        /// Initializes this meta-parser with the provided parser to be wrapped and the wanted endianess.
        /// </summary>
        /// <param name="wrappedParser">The parser being wrapped.</param>
        /// <param name="endianess">The endianess to be forced.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public WithEndianessParser(IBinaryParser<T> wrappedParser, Endianess endianess)
        {
            _wrappedParser = wrappedParser ?? throw new ArgumentNullException(nameof(wrappedParser));
            _endianess = endianess;
        }

        /// <inheritdoc/>
        public Option<int> MinimumByteCount => _wrappedParser.MinimumByteCount;
        /// <inheritdoc/>
        public Option<int> MaxmiumByteCount => _wrappedParser.MaxmiumByteCount;
        /// <inheritdoc/>
        public bool IsFixedSize => _wrappedParser.IsFixedSize;

        /// <inheritdoc/>
        public long CalculateSize(T value) => _wrappedParser.CalculateSize(value);

        /// <inheritdoc/>
        public T Deserialize(Stream stream, IBinaryParsingContext context)
        {
            var previousEndianess = context.Endianess;
            try
            {
                context.Endianess = _endianess;
                return _wrappedParser.Deserialize(stream, context);
            }
            finally
            {
                if (context.Endianess == _endianess)
                    context.Endianess = previousEndianess;
            }
        }

        /// <inheritdoc/>
        public async ValueTask<T> DeserializeAsync(Stream stream, IBinaryParsingContext context, CancellationToken cancellationToken = default)
        {
            var previousEndianess = context.Endianess;
            try
            {
                context.Endianess = _endianess;
                return await _wrappedParser.DeserializeAsync(stream, context, cancellationToken);
            }
            finally
            {
                if (context.Endianess == _endianess)
                    context.Endianess = previousEndianess;
            }
        }

        /// <inheritdoc/>
        public void Serialize(Stream stream, IBinaryParsingContext context, T value)
        {
            var previousEndianess = context.Endianess;
            try
            {
                context.Endianess = _endianess;
                _wrappedParser.Serialize(stream, context, value);
            }
            finally
            {
                if (context.Endianess == _endianess)
                    context.Endianess = previousEndianess;
            }
        }

        /// <inheritdoc/>
        public async ValueTask SerializeAsync(
            Stream stream,
            IBinaryParsingContext context,
            T value,
            CancellationToken cancellationToken = default)
        {
            var previousEndianess = context.Endianess;
            try
            {
                context.Endianess = _endianess;
                await _wrappedParser.SerializeAsync(stream, context, value, cancellationToken);
            }
            finally
            {
                if (context.Endianess == _endianess)
                    context.Endianess = previousEndianess;
            }
        }
    }
}

namespace Tsu.BinaryParser
{
    public static partial class BinaryParserExtensions
    {
        /// <summary>
        /// Forces a parser to use a certain endianess instead of the context's one.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wrappedParser">
        /// <inheritdoc cref="WithEndianessParser{T}.WithEndianessParser(IBinaryParser{T}, Endianess)" path="/param[@name='wrappedParser']"/>
        /// </param>
        /// <param name="endianess">
        /// <inheritdoc cref="WithEndianessParser{T}.WithEndianessParser(IBinaryParser{T}, Endianess)" path="/param[@name='endianess']"/>
        /// </param>
        /// <returns></returns>
        public static WithEndianessParser<T> WithEndianess<T>(this IBinaryParser<T> wrappedParser, Endianess endianess)
        {
            if (wrappedParser is WithByteOrderMark<T>)
                throw new InvalidOperationException("A WithByteOrderMark should not be wrapped in a WithEndianessParser as that'll throw away the Byte Order Mark result.");
            return new WithEndianessParser<T>(wrappedParser, endianess);
        }
    }
}