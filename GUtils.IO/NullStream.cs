/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
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
        public NullStream ( )
        {
        }

        public override Boolean CanRead => false;

        public override Boolean CanSeek => false;

        public override Boolean CanTimeout => false;

        public override Boolean CanWrite => true;

        public override Int64 Length => 0;

        public override Int64 Position
        {
            get => throw new NotSupportedException ( "This stream does not support seek." );
            set => throw new NotSupportedException ( "This stream does not support seek." );
        }

        public override Int64 Seek ( Int64 offset, SeekOrigin origin ) => throw new NotSupportedException ( "This stream does not support seeking." );

        public override void SetLength ( Int64 value ) => throw new NotSupportedException ( "This stream does not support setting it's length." );

        public override void Close ( )
        {
        }

        #region Reading

        public override Int32 Read ( Byte[] buffer, Int32 offset, Int32 count ) => throw new NotSupportedException ( "This stream does not support reading." );

        public override Task<Int32> ReadAsync ( Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken ) => throw new NotSupportedException ( "This stream does not support reading." );

        public override Int32 ReadByte ( ) => throw new NotSupportedException ( "This stream does not support reading." );

        public override IAsyncResult BeginRead ( Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state ) => throw new NotSupportedException ( "This stream does not support reading." );

        public override Int32 EndRead ( IAsyncResult asyncResult ) => throw new NotSupportedException ( "This stream does not support reading." );

        public override Task CopyToAsync ( Stream destination, Int32 bufferSize, CancellationToken cancellationToken ) => throw new NotSupportedException ( "This stream does not support reading." );

        #endregion Reading

        #region Writing

        public override IAsyncResult BeginWrite ( Byte[] buffer, Int32 offset, Int32 count, AsyncCallback callback, Object state ) => Task.CompletedTask;

        public override void EndWrite ( IAsyncResult asyncResult )
        {
        }

        public override void Write ( Byte[] buffer, Int32 offset, Int32 count )
        {
        }

        public override Task WriteAsync ( Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken ) => Task.CompletedTask;

        public override void WriteByte ( Byte value )
        {
        }

        public override void Flush ( )
        {
        }

        public override Task FlushAsync ( CancellationToken cancellationToken ) => Task.CompletedTask;

        #endregion Writing

        #region Object

        public override Boolean Equals ( Object obj ) => obj is NullStream;

        public override Int32 GetHashCode ( ) => base.GetHashCode ( );

        public override String ToString ( ) => "/dev/null stream";

        #endregion Object
    }
}
