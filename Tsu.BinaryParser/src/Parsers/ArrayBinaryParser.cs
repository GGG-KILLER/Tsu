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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tsu.BinaryParser.Parsers;

namespace Tsu.BinaryParser.Parsers
{
    /// <summary>
    /// The builtin length-prefixed array parser.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ArrayBinaryParser<T> : IBinaryParser<List<T>>, IBinaryParser<T[]>, IBinaryParser<IEnumerable<T>>
    {
        private readonly Int32BinaryParser _lengthParser;
        private readonly IBinaryParser<T> _wrappedParser;
        private readonly bool _acceptEofAsEnd;

        /// <summary>
        /// Initializes the builtin parser for nullable types.
        /// </summary>
        /// <param name="wrappedParser">
        /// The parser that will be responsible for parsing the actual type.
        /// </param>
        /// <param name="acceptEofAsEnd">
        /// Whether we should accept an end of stream as the sentinel value.
        /// </param>
        public ArrayBinaryParser(
            IBinaryParser<T> wrappedParser,
            bool acceptEofAsEnd)
        {
            _lengthParser = new Int32BinaryParser(false);
            _wrappedParser = wrappedParser ?? throw new ArgumentNullException(nameof(wrappedParser));
            _acceptEofAsEnd = acceptEofAsEnd;
        }

        /// <remarks>
        /// This always returns 4 which is the amount of bytes used to represent the 0 length of the array.
        /// </remarks>
        public Option<int> MinimumByteCount => 4;
        /// <inheritdoc/>
        /// <remarks>
        /// This always returns None because there is no limit to the size of an array other than <see cref="int.MaxValue"/>.
        /// </remarks>
        public Option<int> MaxmiumByteCount => Option.None<int>();
        /// <inheritdoc/>
        public bool IsFixedSize => false;

        /// <inheritdoc/>
        public long CalculateSize(IEnumerable<T> values)
        {
            long size = 0;
            var count = 0;
            foreach (var value in values)
            {
                size += _wrappedParser.CalculateSize(value);
                count++;
            }
            size += _lengthParser.CalculateSize(count);
            return size;
        }
        long IBinaryParser<T[]>.CalculateSize(T[] values) => CalculateSize(values);
        long IBinaryParser<List<T>>.CalculateSize(List<T> values) => CalculateSize(values);

        /// <inheritdoc/>
        public List<T> Deserialize(IBinaryReader reader, IBinaryParsingContext context)
        {
            var values = new List<T>();
            var count = _lengthParser.Deserialize(reader, context);
            while (!reader.EndOfStream && values.Count < count)
            {
                var value = _wrappedParser.Deserialize(reader, context);
                values.Add(value);
            }

            if (reader.EndOfStream && !_acceptEofAsEnd)
                throw new FormatException("Hit EOF before reading all values for the array.");

            return values;
        }
        T[] IBinaryParser<T[]>.Deserialize(IBinaryReader reader, IBinaryParsingContext context) =>
            Deserialize(reader, context).ToArray();
        IEnumerable<T> IBinaryParser<IEnumerable<T>>.Deserialize(IBinaryReader reader, IBinaryParsingContext context) =>
            Deserialize(reader, context);

        /// <inheritdoc/>
        public async ValueTask<List<T>> DeserializeAsync(IBinaryReader reader, IBinaryParsingContext context, CancellationToken cancellationToken = default)
        {
            var values = new List<T>();
            var count = await _lengthParser.DeserializeAsync(reader, context, cancellationToken);
            while (!reader.EndOfStream && values.Count < count)
            {
                var value = await _wrappedParser.DeserializeAsync(reader, context, cancellationToken);
                values.Add(value);
            }

            if (reader.EndOfStream && !_acceptEofAsEnd)
                throw new FormatException("Hit EOF before reading all values for the array.");

            return values;
        }
        async ValueTask<T[]> IBinaryParser<T[]>.DeserializeAsync(IBinaryReader reader, IBinaryParsingContext context, CancellationToken cancellationToken) =>
            (await DeserializeAsync(reader, context, cancellationToken)).ToArray();
        async ValueTask<IEnumerable<T>> IBinaryParser<IEnumerable<T>>.DeserializeAsync(IBinaryReader reader, IBinaryParsingContext context, CancellationToken cancellationToken) =>
            await DeserializeAsync(reader, context, cancellationToken);

        /// <inheritdoc/>
        public void Serialize(Stream stream, IBinaryParsingContext context, IEnumerable<T> values)
        {
            var count = values.Count();
            _lengthParser.Serialize(stream, context, count);
            foreach (var value in values)
            {
                _wrappedParser.Serialize(stream, context, value);
            }
        }
        void IBinaryParser<List<T>>.Serialize(Stream stream, IBinaryParsingContext context, List<T> values) => Serialize(stream, context, values);
        void IBinaryParser<T[]>.Serialize(Stream stream, IBinaryParsingContext context, T[] values) => Serialize(stream, context, values);

        /// <inheritdoc/>
        public async ValueTask SerializeAsync(Stream stream, IBinaryParsingContext context, IEnumerable<T> values, CancellationToken cancellationToken = default)
        {
            var count = values.Count();
            await _lengthParser.SerializeAsync(stream, context, count, cancellationToken);
            foreach (var value in values)
            {
                await _wrappedParser.SerializeAsync(stream, context, value, cancellationToken);
            }
        }
        ValueTask IBinaryParser<List<T>>.SerializeAsync(Stream stream, IBinaryParsingContext context, List<T> values, CancellationToken cancellationToken) =>
            SerializeAsync(stream, context, values, cancellationToken);
        ValueTask IBinaryParser<T[]>.SerializeAsync(Stream stream, IBinaryParsingContext context, T[] values, CancellationToken cancellationToken) =>
            SerializeAsync(stream, context, values, cancellationToken);
    }
}

namespace Tsu.BinaryParser
{
    public static partial class BinaryParserExtensions
    {
        /// <summary>
        /// Wraps the parser in an <see cref="ArrayBinaryParser{T}"/> parser.
        /// </summary>
        /// <typeparam name="T">
        /// The type which is optional
        /// </typeparam>
        /// <param name="wrappedParser">
        /// <inheritdoc cref="ArrayBinaryParser{T}.ArrayBinaryParser(IBinaryParser{T}, bool)" path="/param[@name='wrappedParser']"/>
        /// </param>
        /// <param name="acceptEofAsEnd">
        /// <inheritdoc cref="ArrayBinaryParser{T}.ArrayBinaryParser(IBinaryParser{T}, bool)" path="/param[@name='acceptEofAsEnd']"/>
        /// </param>
        /// <returns></returns>
        public static ArrayBinaryParser<T> ArrayOf<T>(
            this IBinaryParser<T> wrappedParser,
            bool acceptEofAsEnd = false) =>
            new ArrayBinaryParser<T>(wrappedParser, acceptEofAsEnd);
    }
}
