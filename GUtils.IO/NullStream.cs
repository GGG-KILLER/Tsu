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
        public NullStream ( ) : base ( )
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

        public override Int32 ReadTimeout { get => base.ReadTimeout; set => base.ReadTimeout = value; }

        public override Int32 WriteTimeout
        {
            get => base.WriteTimeout;
            set => base.WriteTimeout = value;
        }

        public override Task CopyToAsync ( Stream destination, Int32 bufferSize, CancellationToken cancellationToken )
        {
            throw new NotSupportedException ( "This stream does not support reading." );
        }

        public override Boolean Equals ( Object obj )
        {
            return obj is NullStream;
        }

        public override void Flush ( )
        {
            // Do nothing
        }

        public override Task FlushAsync ( CancellationToken cancellationToken )
        {
            return Task.CompletedTask;
        }

        public override Int32 GetHashCode ( )
        {
            // All instances of this are one and the same
            return 0;
        }

        public override Int32 Read ( Byte[] buffer, Int32 offset, Int32 count )
        {
            throw new NotSupportedException ( "This stream does not support reading." );
        }

        public override Task<Int32> ReadAsync ( Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken )
        {
            throw new NotSupportedException ( "This stream does not support reading." );
        }

        public override Int32 ReadByte ( )
        {
            throw new NotSupportedException ( "This stream does not support reading." );
        }

        public override Int64 Seek ( Int64 offset, SeekOrigin origin )
        {
            throw new NotSupportedException ( "This stream does not support seeking." );
        }

        public override void SetLength ( Int64 value )
        {
            // Nothing
        }

        public override String ToString ( )
        {
            return "/dev/null stream";
        }

        public override void Write ( Byte[] buffer, Int32 offset, Int32 count )
        {
            // Do nothing either
        }

        public override Task WriteAsync ( Byte[] buffer, Int32 offset, Int32 count, CancellationToken cancellationToken )
        {
            return Task.CompletedTask;
        }

        public override void WriteByte ( Byte value )
        {
            // Do nothing
        }
    }
}
