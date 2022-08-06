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
    /// The builtin parser for nullable types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class NullableParser<T> : IBinaryParser<T?>
    {
        private readonly IBinaryParser<T> _wrappedParser;
        private readonly byte _nullByte;
        private readonly byte _notNullByte;
        private readonly bool _acceptEofAsNull;

        /// <summary>
        /// Initializes the builtin parser for nullable types.
        /// </summary>
        /// <param name="wrappedParser">
        /// The parser that will be responsible for parsing the actual type.
        /// </param>
        /// <param name="nullByte">
        /// The byte we read and write when the value is <see langword="null"/>.
        /// </param>
        /// <param name="notNullByte">
        /// The byte we read and write when the value is not <see langword="null"/>.
        /// </param>
        /// <param name="acceptEofAsNull">
        /// Whether we should accept the stream EOF as <see langword="null"/>.
        /// </param>
        public NullableParser(
            IBinaryParser<T> wrappedParser,
            byte nullByte = 0,
            byte notNullByte = 1,
            bool acceptEofAsNull = true)
        {
            _wrappedParser = wrappedParser ?? throw new ArgumentNullException(nameof(wrappedParser));
            _nullByte = nullByte;
            _notNullByte = notNullByte;
            _acceptEofAsNull = acceptEofAsNull;
        }

        /// <inheritdoc/>
        public Option<int> MinimumByteCount => _wrappedParser.MinimumByteCount.Map(static x => x + 1);
        /// <inheritdoc/>
        public Option<int> MaxmiumByteCount => _wrappedParser.MaxmiumByteCount.Map(static x => x + 1);
        /// <inheritdoc/>
        public bool IsFixedSize => _wrappedParser.IsFixedSize;

        /// <inheritdoc/>
        public long CalculateSize(T? value) => 1 + (value is null ? 0 : _wrappedParser.CalculateSize(value));

        /// <inheritdoc/>
        public T? Deserialize(Stream stream, IBinaryParsingContext context)
        {
            var flag = stream.ReadByte();
            if (flag == _notNullByte)
            {
                return _wrappedParser.Deserialize(stream, context);
            }
            else if (flag == _nullByte || (_acceptEofAsNull && flag == -1))
            {
                return default;
            }
            else
            {
                throw new FormatException("Read value does not indicate neither a null nor non-null value.");
            }
        }

        /// <inheritdoc/>
        public ValueTask<T?> DeserializeAsync(Stream stream, IBinaryParsingContext context, CancellationToken cancellationToken = default)
        {
            var flag = stream.ReadByte();
            if (flag == _notNullByte)
            {
                return _wrappedParser.DeserializeAsync(stream, context, cancellationToken)!;
            }
            else if (flag == _nullByte || (_acceptEofAsNull && flag == -1))
            {
                return new ValueTask<T?>(default(T?));
            }
            else
            {
                throw new FormatException("Read value does not indicate neither a null nor non-null value.");
            }
        }

        /// <inheritdoc/>
        public void Serialize(Stream stream, IBinaryParsingContext context, T? value)
        {
            if (value is null)
            {
                stream.WriteByte(_nullByte);
            }
            else
            {
                stream.WriteByte(_notNullByte);
                _wrappedParser.Serialize(stream, context, value);
            }
        }

        /// <inheritdoc/>
        public ValueTask SerializeAsync(Stream stream, IBinaryParsingContext context, T? value, CancellationToken cancellationToken = default)
        {
            if (value is null)
            {
                stream.WriteByte(_nullByte);
                return new ValueTask();
            }
            else
            {
                stream.WriteByte(_notNullByte);
                return _wrappedParser.SerializeAsync(stream, context, value, cancellationToken);
            }
        }
    }
}

namespace Tsu.BinaryParser
{
    public static partial class BinaryParserExtensions
    {
        /// <summary>
        /// Wraps the parser as an <see cref="Option{T}"/> parser.
        /// </summary>
        /// <typeparam name="T">
        /// The type which is optional
        /// </typeparam>
        /// <param name="wrappedParser">
        /// <inheritdoc cref="NullableParser{T}.NullableParser(IBinaryParser{T}, byte, byte, bool)" path="/param[@name='wrappedParser']"/>
        /// </param>
        /// <param name="noneByte">
        /// <inheritdoc cref="NullableParser{T}.NullableParser(IBinaryParser{T}, byte, byte, bool)" path="/param[@name='noneByte']"/>
        /// </param>
        /// <param name="someByte">
        /// <inheritdoc cref="NullableParser{T}.NullableParser(IBinaryParser{T}, byte, byte, bool)" path="/param[@name='someByte']"/>
        /// </param>
        /// <param name="acceptEofAsNone">
        /// <inheritdoc cref="NullableParser{T}.NullableParser(IBinaryParser{T}, byte, byte, bool)" path="/param[@name='acceptEofAsNone']"/>
        /// </param>
        /// <returns></returns>
        public static NullableParser<T> AsNullable<T>(
            this IBinaryParser<T> wrappedParser,
            byte noneByte = 0,
            byte someByte = 1,
            bool acceptEofAsNone = true) =>
            new NullableParser<T>(wrappedParser, noneByte, someByte, acceptEofAsNone);
    }
}