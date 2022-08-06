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

namespace Tsu.BinaryParser
{
    /// <summary>
    /// A wrapper parser that handles parsing of <see cref="Option{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class OptionParser<T> : IBinaryParser<Option<T>>
    {
        private readonly IBinaryParser<T> _wrappedParser;
        private readonly byte _noneByte;
        private readonly byte _someByte;
        private readonly bool _acceptEofAsNone;

        /// <summary>
        /// Initializes the builtin <see cref="Option{T}"/> parser.
        /// </summary>
        /// <param name="wrappedParser">
        /// The parser that handles the <typeparamref name="T"/>.
        /// </param>
        /// <param name="noneByte"></param>
        /// <param name="someByte"></param>
        /// <param name="acceptEofAsNone"></param>
        public OptionParser(
            IBinaryParser<T> wrappedParser,
            byte noneByte = 0,
            byte someByte = 1,
            bool acceptEofAsNone = true)
        {
            _wrappedParser = wrappedParser ?? throw new ArgumentNullException(nameof(wrappedParser));
            _noneByte = noneByte;
            _someByte = someByte;
            _acceptEofAsNone = acceptEofAsNone;
        }

        /// <inheritdoc/>
        public Option<int> MinimumByteCount => _wrappedParser.MinimumByteCount.Map(x => x + 1);
        /// <inheritdoc/>
        public Option<int> MaxmiumByteCount => _wrappedParser.MaxmiumByteCount.Map(x => x + 1);
        /// <inheritdoc/>
        public bool IsFixedSize => _wrappedParser.IsFixedSize;

        /// <inheritdoc/>
        public long CalculateSize(Option<T> value) => 1 + value.MapOr(0, _wrappedParser.CalculateSize);

        /// <inheritdoc/>
        public Option<T> Deserialize(Stream stream, IBinaryParsingContext context)
        {
            var flag = stream.ReadByte();
            if (flag is not (-1 or 0))
            {
                return _wrappedParser.Deserialize(stream, context);
            }
            return Option.None<T>();
        }

        /// <inheritdoc/>
        public async ValueTask<Option<T>> DeserializeAsync(Stream stream, IBinaryParsingContext context, CancellationToken cancellationToken = default)
        {
            var flag = stream.ReadByte();
            if (flag == _someByte)
            {
                return await _wrappedParser.DeserializeAsync(stream, context, cancellationToken);
            }
            else if (flag == _noneByte || (flag == -1 && _acceptEofAsNone))
            {
                return Option.None<T>();
            }
            else
            {
                throw new FormatException("First byte does not indicate either Some nor None");
            }
        }

        /// <inheritdoc/>
        public void Serialize(Stream stream, IBinaryParsingContext context, Option<T> value)
        {
            if (value.IsNone)
            {
                stream.WriteByte(_noneByte);
            }
            else
            {
                stream.WriteByte(_someByte);
                _wrappedParser.Serialize(stream, context, value.Value);
            }
        }

        /// <inheritdoc/>
        public ValueTask SerializeAsync(
            Stream stream,
            IBinaryParsingContext context,
            Option<T> value,
            CancellationToken cancellationToken = default)
        {
            if (value.IsNone)
            {
                stream.WriteByte(_noneByte);
                return new ValueTask();
            }
            else
            {
                stream.WriteByte(_someByte);
                return _wrappedParser.SerializeAsync(stream, context, value.Value, cancellationToken);
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
        /// <inheritdoc cref="OptionParser{T}.OptionParser(IBinaryParser{T}, byte, byte, bool)" path="/param[@name='wrappedParser']"/>
        /// </param>
        /// <param name="noneByte">
        /// <inheritdoc cref="OptionParser{T}.OptionParser(IBinaryParser{T}, byte, byte, bool)" path="/param[@name='noneByte']"/>
        /// </param>
        /// <param name="someByte">
        /// <inheritdoc cref="OptionParser{T}.OptionParser(IBinaryParser{T}, byte, byte, bool)" path="/param[@name='someByte']"/>
        /// </param>
        /// <param name="acceptEofAsNone">
        /// <inheritdoc cref="OptionParser{T}.OptionParser(IBinaryParser{T}, byte, byte, bool)" path="/param[@name='acceptEofAsNone']"/>
        /// </param>
        /// <returns></returns>
        public static OptionParser<T> AsOptional<T>(
            this IBinaryParser<T> wrappedParser,
            byte noneByte = 0,
            byte someByte = 1,
            bool acceptEofAsNone = true) =>
            new OptionParser<T>(wrappedParser, noneByte, someByte, acceptEofAsNone);
    }
}