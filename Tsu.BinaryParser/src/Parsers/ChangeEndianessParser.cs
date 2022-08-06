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

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tsu.BinaryParser.Parsers
{
    /// <summary>
    /// A meta-parser that changes the endianess for the parsers that follow it.
    /// Produces no result.
    /// </summary>
    public sealed class ChangeEndianessParser : IBinaryParser<Unit>
    {
        private readonly Endianess _endianess;

        /// <summary>
        /// Initializes the new endianess change meta-parser.
        /// </summary>
        /// <param name="endianess"></param>
        public ChangeEndianessParser(Endianess endianess)
        {
            _endianess = endianess;
        }

        /// <inheritdoc/>
        public Option<int> MinimumByteCount => 0;
        /// <inheritdoc/>
        public Option<int> MaxmiumByteCount => 0;
        /// <inheritdoc/>
        public bool IsFixedSize => true;

        /// <inheritdoc/>
        public long CalculateSize(Unit value) => 0;

        /// <inheritdoc/>
        public Unit Deserialize(Stream stream, IBinaryParsingContext context)
        {
            context.Endianess = _endianess;
            return Unit.Value;
        }

        /// <inheritdoc/>
        public ValueTask<Unit> DeserializeAsync(Stream stream, IBinaryParsingContext context, CancellationToken cancellationToken = default)
        {
            context.Endianess = _endianess;
            return new ValueTask<Unit>(Unit.Value);
        }

        /// <inheritdoc/>
        public void Serialize(Stream stream, IBinaryParsingContext context, Unit value) =>
            context.Endianess = _endianess;

        /// <inheritdoc/>
        public ValueTask SerializeAsync(Stream stream, IBinaryParsingContext context, Unit value, CancellationToken cancellationToken = default)
        {
            context.Endianess = _endianess;
            return new ValueTask();
        }
    }
}