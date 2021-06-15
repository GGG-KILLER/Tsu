// Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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

namespace Tsu.IO
{
    /// <summary>
    /// This stream simulates piping data to <pre>/dev/null</pre>
    /// </summary>
    public class NullStream : Stream
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        public NullStream()
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public override bool CanRead => false;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override bool CanSeek => false;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override bool CanTimeout => false;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override bool CanWrite => true;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override long Length => 0;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override long Position
        {
            get => throw new NotSupportedException("This stream does not support seek.");
            set => throw new NotSupportedException("This stream does not support seek.");
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin) =>
            throw new NotSupportedException("This stream does not support seeking.");

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value) =>
            throw new NotSupportedException("This stream does not support setting it's length.");

        /// <summary>
        /// Does nothing
        /// </summary>
        public override void Close()
        {
        }

        #region Reading

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count) =>
            throw new NotSupportedException("This stream does not support reading.");

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
            throw new NotSupportedException("This stream does not support reading.");

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <returns></returns>
        public override int ReadByte() =>
            throw new NotSupportedException("This stream does not support reading.");

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) =>
            throw new NotSupportedException("This stream does not support reading.");

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public override int EndRead(IAsyncResult asyncResult) =>
            throw new NotSupportedException("This stream does not support reading.");

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="bufferSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) =>
            throw new NotSupportedException("This stream does not support reading.");

        #endregion Reading

        #region Writing

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) =>
            Task.CompletedTask;

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="asyncResult"></param>
        public override void EndWrite(IAsyncResult asyncResult)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="value"></param>
        public override void WriteByte(byte value)
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public override void Flush()
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task FlushAsync(CancellationToken cancellationToken) =>
            Task.CompletedTask;

        #endregion Writing

        #region Object

        /// <summary>
        /// Defines whether this stream is equal to another object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) => obj is NullStream;

        /// <summary>
        /// The default hash function
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Prints the string representation of a <see cref="NullStream" />
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "/dev/null stream";

        #endregion Object
    }
}
