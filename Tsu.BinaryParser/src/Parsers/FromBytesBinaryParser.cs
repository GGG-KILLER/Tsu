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

namespace Tsu.BinaryParser.Parsers;

/// <summary>
/// A base class for binary parsers that transform contents from bytes.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class FromBytesBinaryParser<T> : IBinaryParser<T>
{
    private readonly int _size;
    private readonly bool _clearBuffers;

    /// <summary>
    /// Initializes a new instance of this base class.
    /// </summary>
    /// <param name="size">The size that the buffers should be created with.</param>
    /// <param name="clearBuffers">
    /// Whether the rented buffers should be cleared before being returned to their pools.
    /// </param>
    protected FromBytesBinaryParser(int size, bool clearBuffers)
    {
        _size = size;
        _clearBuffers = clearBuffers;
    }

    /// <inheritdoc/>
    public Option<int> MinimumByteCount => _size;
    /// <inheritdoc/>
    public Option<int> MaxmiumByteCount => _size;
    /// <inheritdoc/>
    public bool IsFixedSize => true;

    /// <inheritdoc/>
    public long CalculateSize(T value) => _size;

    /// <summary>
    /// The method to convert the read bytes to the type.
    /// </summary>
    /// <param name="endianess">The endianess the bytes are in.</param>
    /// <param name="buffer">The bytes that were read.</param>
    /// <returns></returns>
    protected abstract T ReadFromBytes(Endianess endianess, Span<byte> buffer);

    /// <summary>
    /// The method to convert the type to bytes.
    /// </summary>
    /// <param name="endianess">The endianess the bytes should be written in.</param>
    /// <param name="buffer">The buffer to write the bytes into.</param>
    /// <param name="value">The value to write into the buffer.</param>
    protected abstract void WriteToBytes(Endianess endianess, Span<byte> buffer, T value);

    /// <inheritdoc/>
    public T Deserialize(IBinaryReader reader, IBinaryParsingContext context)
    {
        const int requiredBytes = sizeof(short);

#if HAS_SPAN
        Span<byte> bytes = stackalloc byte[requiredBytes];
        reader.Read(bytes);
#else
        using var buffer = ByteBufferPool.Rent(requiredBytes, _clearBuffers);
        buffer.FillFrom(reader);
        var bytes = buffer.Span;
#endif

        return ReadFromBytes(context.Endianess, bytes);
    }

    /// <inheritdoc/>
    public async ValueTask<T> DeserializeAsync(
        IBinaryReader reader,
        IBinaryParsingContext context,
        CancellationToken cancellationToken = default)
    {
        using var buffer = ByteBufferPool.Rent(sizeof(short), _clearBuffers);
        await buffer.FillFromAsync(reader, cancellationToken);
        return ReadFromBytes(context.Endianess, buffer.Span);
    }

    /// <inheritdoc/>
    public void Serialize(Stream stream, IBinaryParsingContext context, T value)
    {
#if HAS_SPAN
        Span<byte> span = stackalloc byte[sizeof(short)];
#else
        using var buffer = ByteBufferPool.Rent(sizeof(short));
        var span = buffer.Span;
#endif

        WriteToBytes(context.Endianess, span, value);

#if HAS_SPAN
        stream.Write(span);
#else
        buffer.WriteTo(stream);
#endif
    }

    /// <inheritdoc/>
    public async ValueTask SerializeAsync(
        Stream stream,
        IBinaryParsingContext context,
        T value,
        CancellationToken cancellationToken = default)
    {
        using var buffer = ByteBufferPool.Rent(sizeof(short));
        WriteToBytes(context.Endianess, buffer.Span, value);
        await buffer.WriteToAsync(stream, cancellationToken);
    }
}
