/*
 * Copyright © 2019 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the “Software”), to deal in the Software without
 * restriction, including without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
 * the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GUtils.IO
{
    /// <summary>
    /// This stream simulates piping data to <pre>/dev/null</pre>
    /// </summary>
    public class NullStream : Stream
    {
        /// <summary>
        /// Does nothing
        /// </summary>
        public NullStream ( )
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public override Boolean CanRead => false;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override Boolean CanSeek => false;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override Boolean CanTimeout => false;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override Boolean CanWrite => true;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override Int64 Length => 0;

        /// <summary>
        /// Does nothing
        /// </summary>
        public override Int64 Position
        {
            get => throw new NotSupportedException ( "This stream does not support seek." );
            set => throw new NotSupportedException ( "This stream does not support seek." );
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override Int64 Seek ( Int64 offset, SeekOrigin origin ) =>
            throw new NotSupportedException ( "This stream does not support seeking." );

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength ( Int64 value ) =>
            throw new NotSupportedException ( "This stream does not support setting it's length." );

        /// <summary>
        /// Does nothing
        /// </summary>
        public override void Close ( )
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
        public override Int32 Read ( Byte[] buffer, Int32 offset, Int32 count ) =>
            throw new NotSupportedException ( "This stream does not support reading." );

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<Int32> ReadAsync ( Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken ) =>
            throw new NotSupportedException ( "This stream does not support reading." );

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <returns></returns>
        public override Int32 ReadByte ( ) =>
            throw new NotSupportedException ( "This stream does not support reading." );

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public override IAsyncResult BeginRead ( Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state ) =>
            throw new NotSupportedException ( "This stream does not support reading." );

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="asyncResult"></param>
        /// <returns></returns>
        public override Int32 EndRead ( IAsyncResult asyncResult ) =>
            throw new NotSupportedException ( "This stream does not support reading." );

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="bufferSize"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task CopyToAsync ( Stream destination, Int32 bufferSize, CancellationToken cancellationToken ) =>
            throw new NotSupportedException ( "This stream does not support reading." );

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
        public override IAsyncResult BeginWrite ( Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state ) => Task.CompletedTask;

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="asyncResult"></param>
        public override void EndWrite ( IAsyncResult asyncResult )
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write ( Byte[] buffer, Int32 offset, Int32 count )
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
        public override Task WriteAsync ( Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken ) => Task.CompletedTask;

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="value"></param>
        public override void WriteByte ( Byte value )
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        public override void Flush ( )
        {
        }

        /// <summary>
        /// Does nothing
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task FlushAsync ( CancellationToken cancellationToken ) => Task.CompletedTask;

        #endregion Writing

        #region Object

        /// <summary>
        /// Defines whether this stream is equal to another object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Boolean Equals ( Object obj ) => obj is NullStream;

        /// <summary>
        /// The default hash function
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode ( ) => base.GetHashCode ( );

        /// <summary>
        /// Prints the string representation of a <see cref="NullStream" />
        /// </summary>
        /// <returns></returns>
        public override String ToString ( ) => "/dev/null stream";

        #endregion Object
    }
}
