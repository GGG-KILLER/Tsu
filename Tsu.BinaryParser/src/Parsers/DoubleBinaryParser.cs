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
using System.Runtime.InteropServices;

namespace Tsu.BinaryParser.Parsers;

/// <summary>
/// The builtin <see cref="float"/> parser.
/// </summary>
public sealed class DoubleBinaryParser : FromBytesBinaryParser<double>
{
    /// <summary>
    /// Initializes the builtin <see cref="double"/> parser.
    /// </summary>
    /// <param name="clearBuffers">
    /// <inheritdoc cref="FromBytesBinaryParser{T}.FromBytesBinaryParser(int, bool)" path="/param[@name='clearBuffers']"/>
    /// </param>
    public DoubleBinaryParser(bool clearBuffers) : base(sizeof(double), clearBuffers)
    {
    }

    /// <inheritdoc/>
    protected override double ReadFromBytes(Endianess endianess, Span<byte> buffer)
    {
        if ((!BitConverter.IsLittleEndian && endianess == Endianess.LittleEndian)
            || (BitConverter.IsLittleEndian && endianess == Endianess.BigEndian))
        {
            SpanHelpers.Reverse(buffer);
        }
        return MemoryMarshal.Read<double>(buffer);
    }

    /// <inheritdoc/>
    protected override void WriteToBytes(Endianess endianess, Span<byte> buffer, double value)
    {
        MemoryMarshal.Write(buffer, ref value);
        if ((!BitConverter.IsLittleEndian && endianess == Endianess.LittleEndian)
            || (BitConverter.IsLittleEndian && endianess == Endianess.BigEndian))
        {
            SpanHelpers.Reverse(buffer);
        }
    }
}