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

namespace Tsu.BinaryParser;

/// <summary>
/// A reader for binary data.
/// </summary>
public interface IBinaryReader : IDisposable
{
    /// <summary>
    /// Whether we've hit the end of the stream.
    /// </summary>
    bool EndOfStream { get; }

    /// <summary>
    /// Attempts to peek a single byte from the stream.
    /// </summary>
    Option<byte> PeekByte();

    /// <summary>
    /// Attempts to read a single byte from the stream.
    /// </summary>
    Option<byte> ReadByte();

    /// <summary>
    /// Reads the specified amount of bytes from the stream.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to read the requested amount of bytes into.
    /// </param>
    /// <param name="bytesToRead">
    /// The amount of bytes
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="bytesToRead"/> is larger than the length
    /// of <paramref name="buffer"/>.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown when the end of the stream is hit.
    /// </exception>
    void Read(byte[] buffer, int bytesToRead);

    /// <summary>
    /// Reads the amount of bytes the span can hold into the provided buffer.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to read the bytes into.
    /// </param>
    /// <exception cref="EndOfStreamException">
    /// Thrown when the end of the stream is hit.
    /// </exception>
    void Read(Span<byte> buffer);

    /// <summary>
    /// Reads the specified amount of bytes from the stream asynchronously if
    /// not enough data has been buffered.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to read the requested amount of bytes into.
    /// </param>
    /// <param name="bytesToRead">
    /// The amount of bytes
    /// </param>
    /// <param name="cancellationToken">
    /// The token to cancel the operation.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="bytesToRead"/> is larger than the length
    /// of <paramref name="buffer"/>.
    /// </exception>
    /// <exception cref="EndOfStreamException">
    /// Thrown when the end of the stream is hit.
    /// </exception>
    ValueTask ReadAsync(byte[] buffer, int bytesToRead, CancellationToken cancellationToken);

    /// <summary>
    /// Reads the amount of bytes the memory can hold into the provided buffer
    /// asynchronously if not enough data has been buffered.
    /// </summary>
    /// <param name="buffer">
    /// The buffer to read the bytes into.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to cancel the operation.
    /// </param>
    /// <exception cref="EndOfStreamException">
    /// Thrown when the end of the stream is hit.
    /// </exception>
    ValueTask ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken);
}
