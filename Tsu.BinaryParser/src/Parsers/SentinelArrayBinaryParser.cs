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
    /// The builtin sentinel-based array parser.
    /// <para>This is <b>NOT RECOMMENDED</b> for creating your own file formats, only use it to parse existing file formats.</para>
    /// <para>Always prefer the length-prefixed array parser for creating your own file formats instead.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class SentinelArrayBinaryParser<T> : IBinaryParser<List<T>>, IBinaryParser<T[]>, IBinaryParser<IEnumerable<T>>
    {
        private readonly IBinaryParser<T> _wrappedParser;
        private readonly T _sentinelValue;
        private readonly bool _includeSentinelInTheArray, _acceptEofAsSentinel;

        /// <summary>
        /// Initializes the builtin parser for nullable types.
        /// </summary>
        /// <param name="wrappedParser">
        /// The parser that will be responsible for parsing the actual type.
        /// </param>
        /// <param name="sentinelValue">
        /// The sentinel value we should stop reading at.
        /// </param>
        /// <param name="includeSentinelInTheArray">
        /// Whether the sentinel value should be included in the parsed array.
        /// <param name="acceptEofAsSentinel">
        /// </param>
        /// Whether we should accept an end of stream as the sentinel value.
        /// </param>
        public SentinelArrayBinaryParser(
            IBinaryParser<T> wrappedParser,
            T sentinelValue,
            bool includeSentinelInTheArray,
            bool acceptEofAsSentinel)
        {
            _wrappedParser = wrappedParser ?? throw new ArgumentNullException(nameof(wrappedParser));
            _sentinelValue = sentinelValue;
            _includeSentinelInTheArray = includeSentinelInTheArray;
            _acceptEofAsSentinel = acceptEofAsSentinel;
        }

        /// <inheritdoc/>
        public Option<int> MinimumByteCount => _wrappedParser.MinimumByteCount;
        /// <inheritdoc/>
        /// <remarks>
        /// This always returns None because there is no sane limit to the size of an array other than <see cref="int.MaxValue"/>.
        /// </remarks>
        public Option<int> MaxmiumByteCount => Option.None<int>();
        /// <inheritdoc/>
        public bool IsFixedSize => false;

        /// <inheritdoc/>
        public long CalculateSize(IEnumerable<T> values)
        {
            var length = values.Sum(_wrappedParser.CalculateSize);
            if (!EqualityComparer<T>.Default.Equals(_sentinelValue, values.Last()))
                length += _wrappedParser.CalculateSize(_sentinelValue);
            return length;
        }
        long IBinaryParser<T[]>.CalculateSize(T[] values) => CalculateSize(values);
        long IBinaryParser<List<T>>.CalculateSize(List<T> values) => CalculateSize(values);

        /// <inheritdoc/>
        public List<T> Deserialize(IBinaryReader reader, IBinaryParsingContext context)
        {
            var values = new List<T>();
            var foundSentinel = false;
            while (!reader.EndOfStream)
            {
                var value = _wrappedParser.Deserialize(reader, context);
                if (EqualityComparer<T>.Default.Equals(_sentinelValue, value))
                {
                    foundSentinel = true;
                    if (_includeSentinelInTheArray)
                        values.Add(value);
                    break;
                }
                values.Add(value);
            }

            if (!foundSentinel && reader.EndOfStream && !_acceptEofAsSentinel)
                throw new FormatException("Hit EOF before finding end of array value.");

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
            var foundSentinel = false;
            while (!reader.EndOfStream)
            {
                var value = await _wrappedParser.DeserializeAsync(reader, context, cancellationToken);
                if (EqualityComparer<T>.Default.Equals(_sentinelValue, value))
                {
                    foundSentinel = true;
                    if (_includeSentinelInTheArray)
                        values.Add(value);
                    break;
                }
                values.Add(value);
            }

            if (!foundSentinel && reader.EndOfStream && !_acceptEofAsSentinel)
                throw new FormatException("Hit EOF before finding end of array value.");

            return values;
        }
        async ValueTask<T[]> IBinaryParser<T[]>.DeserializeAsync(IBinaryReader reader, IBinaryParsingContext context, CancellationToken cancellationToken) =>
            (await DeserializeAsync(reader, context, cancellationToken)).ToArray();
        async ValueTask<IEnumerable<T>> IBinaryParser<IEnumerable<T>>.DeserializeAsync(IBinaryReader reader, IBinaryParsingContext context, CancellationToken cancellationToken) =>
            await DeserializeAsync(reader, context, cancellationToken);

        /// <inheritdoc/>
        public void Serialize(Stream stream, IBinaryParsingContext context, IEnumerable<T> values)
        {
            Option<T> last = Option.None<T>();
            foreach (var value in values)
            {
                _wrappedParser.Serialize(stream, context, value);
                last = value;
            }
            if (last.IsNone || !EqualityComparer<T>.Default.Equals(last.Value, _sentinelValue))
                _wrappedParser.Serialize(stream, context, _sentinelValue);
        }
        void IBinaryParser<List<T>>.Serialize(Stream stream, IBinaryParsingContext context, List<T> values) => Serialize(stream, context, values);
        void IBinaryParser<T[]>.Serialize(Stream stream, IBinaryParsingContext context, T[] values) => Serialize(stream, context, values);

        /// <inheritdoc/>
        public async ValueTask SerializeAsync(Stream stream, IBinaryParsingContext context, IEnumerable<T> values, CancellationToken cancellationToken = default)
        {
            Option<T> last = Option.None<T>();
            foreach (var value in values)
            {
                await _wrappedParser.SerializeAsync(stream, context, value, cancellationToken);
                last = value;
            }
            if (last.IsNone || !EqualityComparer<T>.Default.Equals(last.Value, _sentinelValue))
                await _wrappedParser.SerializeAsync(stream, context, _sentinelValue, cancellationToken);
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
        /// Wraps the parser in a <see cref="SentinelArrayBinaryParser{T}"/> parser.
        /// </summary>
        /// <typeparam name="T">
        /// The type which is optional
        /// </typeparam>
        /// <param name="wrappedParser">
        /// <inheritdoc cref="SentinelArrayBinaryParser{T}.SentinelArrayBinaryParser(IBinaryParser{T}, T, bool, bool)" path="/param[@name='wrappedParser']"/>
        /// </param>
        /// <param name="sentinelValue">
        /// <inheritdoc cref="SentinelArrayBinaryParser{T}.SentinelArrayBinaryParser(IBinaryParser{T}, T, bool, bool)" path="/param[@name='sentinelValue']"/>
        /// </param>
        /// <param name="includeSentinelInTheArray">
        /// <inheritdoc cref="SentinelArrayBinaryParser{T}.SentinelArrayBinaryParser(IBinaryParser{T}, T, bool, bool)" path="/param[@name='includeSentinelInTheArray']"/>
        /// </param>
        /// <param name="acceptEofAsSentinel">
        /// <inheritdoc cref="SentinelArrayBinaryParser{T}.SentinelArrayBinaryParser(IBinaryParser{T}, T, bool, bool)" path="/param[@name='acceptEofAsSentinel']"/>
        /// </param>
        /// <returns></returns>
        public static SentinelArrayBinaryParser<T> SentinelArrayOf<T>(
            this IBinaryParser<T> wrappedParser,
            T sentinelValue,
            bool includeSentinelInTheArray = false,
            bool acceptEofAsSentinel = true) =>
            new SentinelArrayBinaryParser<T>(wrappedParser, sentinelValue, includeSentinelInTheArray, acceptEofAsSentinel);
    }
}