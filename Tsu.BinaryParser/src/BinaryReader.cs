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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Tsu.BinaryParser;

/// <summary>
/// Our default implementation of <see cref="IBinaryReader"/>.
/// </summary>
// This was heavily inspired by .NET's StreamReader.
public sealed class BinaryReader : IBinaryReader
{
    /// <summary>
    /// The default value for the bufferSize parameter in this class' constructors.
    /// </summary>
    public const int DefaultBufferSize = 4096;

    /// <summary>
    /// The default value for the leaveOpen parameter in this class' constructors.
    /// </summary>
    public const bool DefaultLeaveOpen = false;

    private readonly bool _leaveOpen;
    private Stream? _stream;
    private byte[]? _buffer;
    private bool _disposedValue;
    private int _bufferPos, _bufferLen;

    /// <summary>
    /// Initializes a new <see cref="BinaryReader"/> with the <see cref="DefaultBufferSize"/>
    /// and leaveOpen set to <see langword="false"/>.
    /// </summary>
    /// <param name="stream">
    /// <inheritdoc cref="BinaryReader(Stream, int, bool)" path="/param[@name='stream']"/>
    /// </param>
    public BinaryReader(Stream stream) : this(stream, DefaultBufferSize, DefaultLeaveOpen)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="BinaryReader"/> with the provided buffer size and leaveOpen
    /// set to <see langword="false"/>.
    /// </summary>
    /// <param name="stream">
    /// <inheritdoc cref="BinaryReader(Stream, int, bool)" path="/param[@name='stream']"/>
    /// </param>
    /// <param name="bufferSize">
    /// <inheritdoc cref="BinaryReader(Stream, int, bool)" path="/param[@name='bufferSize']"/>
    /// </param>
    public BinaryReader(Stream stream, int bufferSize) : this(stream, bufferSize, DefaultLeaveOpen)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="BinaryReader"/> with the provided buffer size and leaveOpen.
    /// </summary>
    /// <param name="stream">
    /// The stream to read data from.
    /// </param>
    /// <param name="bufferSize">
    /// The size of the buffer to buffer data into.
    /// </param>
    /// <param name="leaveOpen">
    /// Whether to leave the stream open after disposing.
    /// </param>
    public BinaryReader(Stream stream, int bufferSize, bool leaveOpen)
    {
        _stream = stream;
        _buffer = new byte[bufferSize];
        _leaveOpen = leaveOpen;
    }

    /// <inheritdoc/>
    public bool EndOfStream
    {
        get
        {
            ThrowIfDisposed();

            if (_bufferLen < _bufferPos)
                return false;

            var read = FillBuffer();
            return read == 0;
        }
    }

    /// <inheritdoc/>
    public Option<byte> PeekByte()
    {
        ThrowIfDisposed();

        if (_bufferPos == _bufferLen && FillBuffer() == 0)
            return Option.None<byte>();

        return _buffer[_bufferPos];
    }

    /// <inheritdoc/>
    public Option<byte> ReadByte()
    {
        ThrowIfDisposed();

        if (_bufferPos == _bufferLen && FillBuffer() == 0)
            return Option.None<byte>();

        var value = _buffer[_bufferPos];
        _bufferPos++;
        return value;
    }

    /// <inheritdoc/>
    public void Read(byte[] buffer, int bytesToRead)
    {
        ThrowIfDisposed();

        var bytesLeft = bytesToRead;
        while (bytesLeft > 0)
        {
            if (_bufferPos == _bufferLen && FillBuffer() == 0)
                throw new EndOfStreamException("Unable to read the requested amount of bytes from the stream.");

            var bytesToCopy = Math.Min(_bufferLen - _bufferPos, bytesLeft);
            Buffer.BlockCopy(_buffer, _bufferPos, buffer, bytesLeft - bytesToRead, bytesToCopy);
            bytesLeft -= bytesToCopy;
            _bufferPos += bytesToCopy;
        }
    }

    /// <inheritdoc/>
    public void Read(Span<byte> buffer)
    {
        ThrowIfDisposed();

        var bytesLeft = buffer.Length;
        while (bytesLeft > 0)
        {
            if (_bufferPos == _bufferLen && FillBuffer() == 0)
                throw new EndOfStreamException("Unable to read the requested amount of bytes from the stream.");

            var bytesToCopy = Math.Min(_bufferLen - _bufferPos, bytesLeft);
            var internalBuffer = new Span<byte>(_buffer, _bufferPos, bytesToCopy);
            internalBuffer.CopyTo(buffer.Slice(bytesLeft - buffer.Length, bytesToCopy));
            bytesLeft -= bytesToCopy;
            _bufferPos += bytesToCopy;
        }
    }

    /// <inheritdoc/>
    public ValueTask ReadAsync(byte[] buffer, int bytesToRead, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        return Core(buffer, bytesToRead, cancellationToken);

        async ValueTask Core(byte[] buffer, int bytesToRead, CancellationToken cancellationToken)
        {
            var bytesLeft = bytesToRead;
            while (bytesLeft > 0)
            {
                if (_bufferPos == _bufferLen && await FillBufferAsync(cancellationToken) == 0)
                    throw new EndOfStreamException("Unable to read the requested amount of bytes from the stream.");

                var bytesToCopy = Math.Min(_bufferLen - _bufferPos, bytesLeft);
                Buffer.BlockCopy(_buffer, _bufferPos, buffer, bytesLeft - bytesToRead, bytesToCopy);
                bytesLeft -= bytesToCopy;
                _bufferPos += bytesToCopy;
            }
        }
    }

    /// <inheritdoc/>
    public ValueTask ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        return Core(buffer, cancellationToken);

        async ValueTask Core(Memory<byte> buffer, CancellationToken cancellationToken)
        {
            var bytesLeft = buffer.Length;
            while (bytesLeft > 0)
            {
                if (_bufferPos == _bufferLen && await FillBufferAsync(cancellationToken) == 0)
                    throw new EndOfStreamException("Unable to read the requested amount of bytes from the stream.");

                var bytesToCopy = Math.Min(_bufferLen - _bufferPos, bytesLeft);
                _buffer.AsSpan(_bufferPos, bytesToCopy)
                       .CopyTo(buffer.Slice(bytesLeft - buffer.Length, bytesToCopy).Span);
                bytesLeft -= bytesToCopy;
                _bufferPos += bytesToCopy;
            }
        }
    }

    [MemberNotNull(nameof(_stream), nameof(_buffer))]
    private void ThrowIfDisposed()
    {
        if (_disposedValue)
            throw new InvalidOperationException("Object has been disposed.");
        InternalDebug.AssertNotNull(_stream);
        InternalDebug.AssertNotNull(_buffer);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int FillBuffer()
    {
        ThrowIfDisposed();

        _bufferPos = 0;
        _bufferLen = _stream.Read(_buffer, 0, _buffer.Length);

        return _bufferLen;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Task<int> FillBufferAsync(CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        return Core(cancellationToken);

        async Task<int> Core(CancellationToken cancellationToken)
        {
            _bufferPos = 0;
            _bufferLen = await _stream.ReadAsync(_buffer, 0, _buffer.Length, cancellationToken)
                                      .ConfigureAwait(false);

            return _bufferLen;
        }
    }

    #region IDisposable

    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing && !_leaveOpen)
                _stream!.Dispose();

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _buffer = null;
            _stream = null;
            _disposedValue = true;
        }
    }

    /// <summary>
    /// Finalizes this object.
    /// </summary>
    ~BinaryReader()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion IDisposable
}