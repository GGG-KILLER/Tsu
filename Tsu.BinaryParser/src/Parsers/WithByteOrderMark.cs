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
using System.Threading;
using System.Threading.Tasks;
using Tsu.BinaryParser.Parsers;

namespace Tsu.BinaryParser
{
    /// <summary>
    /// A meta-parser to wrap another parser and set the endianess based on the parsed value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class WithByteOrderMark<T> : IBinaryParser<T>
    {
        private readonly IBinaryParser<T> _wrappedParser;
        private readonly IEqualityComparer<T> _equalityComparer;
        private readonly T _littleEndianValue;
        private readonly T _bigEndianValue;

        /// <summary>
        /// Initializes a new byte order mark meta-parser.
        /// </summary>
        /// <param name="wrappedParser">
        /// The parser that will actually read the value for the Byte Order Mark.
        /// </param>
        /// <param name="equalityComparer">
        /// The equality comparer to use when checking if the parsed value is equal to either provided one.
        /// </param>
        /// <param name="littleEndianValue">
        /// The value that indicates the following data is in little endian.
        /// </param>
        /// <param name="bigEndianValue">
        /// The value that indicates the following data is in big endian.
        /// </param>
        public WithByteOrderMark(IBinaryParser<T> wrappedParser, IEqualityComparer<T> equalityComparer, T littleEndianValue, T bigEndianValue)
        {
            _wrappedParser = wrappedParser ?? throw new ArgumentNullException(nameof(wrappedParser));
            _equalityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(wrappedParser));
            if (_equalityComparer.Equals(littleEndianValue, bigEndianValue))
                throw new ArgumentException("The little endian and big endian values must be different.", nameof(littleEndianValue));
            _littleEndianValue = littleEndianValue;
            _bigEndianValue = bigEndianValue;
        }

        /// <inheritdoc cref="WithByteOrderMark{T}.WithByteOrderMark(IBinaryParser{T}, IEqualityComparer{T}, T, T)"/>
        public WithByteOrderMark(IBinaryParser<T> wrappedParser, T littleEndianValue, T bigEndianValue)
            : this(wrappedParser, EqualityComparer<T>.Default, littleEndianValue, bigEndianValue)
        {
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
            var value = _wrappedParser.Deserialize(stream, context);
            if (_equalityComparer.Equals(value, _littleEndianValue))
            {
                context.Endianess = Endianess.LittleEndian;
            }
            else if (_equalityComparer.Equals(value, _bigEndianValue))
            {
                context.Endianess = Endianess.BigEndian;
            }
            else
            {
                throw new FormatException("The value for the Byte Order Mark does not indicate either little endian or big endian.");
            }
            return value;
        }

        /// <inheritdoc/>
        public async ValueTask<T> DeserializeAsync(Stream stream, IBinaryParsingContext context, CancellationToken cancellationToken = default)
        {
            var value = await _wrappedParser.DeserializeAsync(stream, context, cancellationToken);
            if (_equalityComparer.Equals(value, _littleEndianValue))
            {
                context.Endianess = Endianess.LittleEndian;
            }
            else if (_equalityComparer.Equals(value, _bigEndianValue))
            {
                context.Endianess = Endianess.BigEndian;
            }
            else
            {
                throw new FormatException("The value for the Byte Order Mark does not indicate either little endian or big endian.");
            }
            return value;
        }

        /// <inheritdoc/>
        public void Serialize(Stream stream, IBinaryParsingContext context, T value)
        {
            _wrappedParser.Serialize(
                stream,
                context,
                context.Endianess == Endianess.BigEndian ? _bigEndianValue : _littleEndianValue);
        }

        /// <inheritdoc/>
        public ValueTask SerializeAsync(Stream stream, IBinaryParsingContext context, T value, CancellationToken cancellationToken = default)
        {
            return _wrappedParser.SerializeAsync(
                stream,
                context,
                context.Endianess == Endianess.BigEndian ? _bigEndianValue : _littleEndianValue,
                cancellationToken);
        }
    }
}

namespace Tsu.BinaryParser
{
    public static partial class BinaryParserExtensions
    {
        /// <summary>
        /// Marks the a parser as indicator of endianess for the following data.
        /// <para>DO NOT WRAP THIS IN A <see cref="WithEndianessParser{T}"/></para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wrappedParser">
        /// <inheritdoc cref="WithByteOrderMark{T}.WithByteOrderMark(IBinaryParser{T}, T, T)" path="/param[@name='wrappedParser']"/>
        /// </param>
        /// <param name="littleEndianValue">
        /// <inheritdoc cref="WithByteOrderMark{T}.WithByteOrderMark(IBinaryParser{T}, T, T)" path="/param[@name='littleEndianValue']"/>
        /// </param>
        /// <param name="bigEndianValue">
        /// <inheritdoc cref="WithByteOrderMark{T}.WithByteOrderMark(IBinaryParser{T}, T, T)" path="/param[@name='bigEndianValue']"/>
        /// </param>
        /// <returns></returns>
        public static WithByteOrderMark<T> IndicatesByteOrderMark<T>(this IBinaryParser<T> wrappedParser, T littleEndianValue, T bigEndianValue) =>
            new WithByteOrderMark<T>(wrappedParser, littleEndianValue, bigEndianValue);
    }
}