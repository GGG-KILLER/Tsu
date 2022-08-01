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
using System.Buffers.Binary;

namespace Tsu.BinaryParser.Parsers;

/// <summary>
/// The builtin <see cref="long"/> parser.
/// </summary>
public sealed class Int64BinaryParser : FromBytesBinaryParser<long>
{
    /// <summary>
    /// Initializes the builtin <see cref="long"/> parser.
    /// </summary>
    /// <param name="clearBuffers">
    /// <inheritdoc cref="FromBytesBinaryParser{T}.FromBytesBinaryParser(int, bool)" path="/param[@name='clearBuffers']"/>
    /// </param>
    public Int64BinaryParser(bool clearBuffers) : base(sizeof(long), clearBuffers)
    {
    }

    /// <inheritdoc/>
    protected override long ReadFromBytes(Endianess endianess, Span<byte> buffer) =>
        endianess == Endianess.BigEndian
        ? BinaryPrimitives.ReadInt64BigEndian(buffer)
        : BinaryPrimitives.ReadInt64LittleEndian(buffer);

    /// <inheritdoc/>
    protected override void WriteToBytes(Endianess endianess, Span<byte> buffer, long value)
    {
        if (endianess == Endianess.BigEndian)
            BinaryPrimitives.WriteInt64BigEndian(buffer, value);
        else
            BinaryPrimitives.WriteInt64LittleEndian(buffer, value);
    }
}
