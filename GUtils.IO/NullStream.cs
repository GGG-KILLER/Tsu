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
