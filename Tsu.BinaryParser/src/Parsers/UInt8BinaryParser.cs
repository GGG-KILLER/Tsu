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

namespace Tsu.BinaryParser.Parsers;

/// <summary>
/// The builtin <see cref="byte"/> parser.
/// </summary>
public sealed class UInt8BinaryParser : IBinaryParser<byte>
{
    /// <inheritdoc/>
    public Option<int> MinimumByteCount => sizeof(byte);

    /// <inheritdoc/>
    public Option<int> MaxmiumByteCount => sizeof(byte);

    /// <inheritdoc/>
    public bool IsFixedSize => true;

    /// <inheritdoc/>
    public long CalculateSize(byte value) => sizeof(byte);

    /// <inheritdoc/>
    public byte Deserialize(Stream stream, IBinaryParsingContext context)
    {
        var val = stream.ReadByte();
        if (val == -1)
            throw new EndOfStreamException();
        return (byte) val;
    }

    /// <inheritdoc/>
    public ValueTask<byte> DeserializeAsync(Stream stream, IBinaryParsingContext context, CancellationToken cancellationToken = default)
    {
        var val = stream.ReadByte();
        if (val == -1)
            throw new EndOfStreamException();
        return new ValueTask<byte>((byte) val);
    }

    /// <inheritdoc/>
    public void Serialize(Stream stream, IBinaryParsingContext context, byte value) =>
        stream.WriteByte(value);

    /// <inheritdoc/>
    public ValueTask SerializeAsync(Stream stream, IBinaryParsingContext context, byte value, CancellationToken cancellationToken = default)
    {
        stream.WriteByte(value);
        return new ValueTask();
    }
}
