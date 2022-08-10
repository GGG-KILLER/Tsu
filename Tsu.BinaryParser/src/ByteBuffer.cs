using System;
using System.Buffers;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Tsu.BinaryParser;

/// <summary>
/// Represents a HEAP allocated byte buffer.
/// <para>For stack allocated ones, use <see cref="Span{T}"/>.</para>
/// </summary>
internal readonly struct ByteBuffer : IDisposable
{
    private readonly bool _clearWhenReturning;
    public readonly int Length;

#if HAS_SPAN
    internal readonly IMemoryOwner<byte> _memoryOwner;

    public ByteBuffer(IMemoryOwner<byte> memoryOwner, int size, bool clearWhenReturning)
    {
        _memoryOwner = memoryOwner;
        Length = size;
        _clearWhenReturning = clearWhenReturning;
    }

    public Memory<byte> Memory => _memoryOwner.Memory.Slice(0, Length);
    public Span<byte> Span => Memory.Span;

    public void Dispose()
    {
        if (_clearWhenReturning)
            _memoryOwner.Memory.Span.Fill(0);
        _memoryOwner.Dispose();
    }
#else
    internal readonly byte[] _buffer;

    public ByteBuffer(byte[] buffer, int size, bool clearWhenReturning)
    {
        _buffer = buffer;
        Length = size;
        _clearWhenReturning = clearWhenReturning;
    }

    public Memory<byte> Memory => _buffer.AsMemory(0, Length);
    public Span<byte> Span => _buffer.AsSpan(0, Length);

    public void Dispose() => ArrayPool<byte>.Shared.Return(_buffer, _clearWhenReturning);
#endif

    public void FillFrom(Stream stream)
    {
#if HAS_SPAN
        var readBytes = stream.Read(Span);
#else
        var readBytes = stream.Read(_buffer, 0, Length);
#endif

        if (readBytes != Length)
            throw new EndOfStreamException();
    }

    public async ValueTask FillFromAsync(Stream stream, CancellationToken cancellationToken = default)
    {
#if HAS_SPAN
        var readBytes = await stream.ReadAsync(_memoryOwner.Memory.Slice(0, Length), cancellationToken);
#else
        var readBytes = await stream.ReadAsync(_buffer, 0, Length, cancellationToken);
#endif

        if (readBytes != Length)
            throw new EndOfStreamException();
    }

    public void WriteTo(Stream stream) =>
#if HAS_SPAN
        stream.Write(Span);
#else
        stream.Write(_buffer, 0, Length);
#endif

    public async ValueTask WriteToAsync(Stream stream, CancellationToken cancellationToken = default) =>
#if HAS_SPAN
        await stream.WriteAsync(Memory, cancellationToken);
#else
        await stream.WriteAsync(_buffer, 0, Length, cancellationToken);
#endif

}
