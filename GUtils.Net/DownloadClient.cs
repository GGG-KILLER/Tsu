using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GUtils.Net
{
	public struct DownloadClientDownloadProgressChangedArgs
	{
		public Int64 BytesReceived;
		public Int64 TotalBytes;
	}

	public delegate void DownloadClientDownloadProgressChangedHandler ( Object sender, DownloadClientDownloadProgressChangedArgs e );

	public class DownloadClient
	{
		private readonly Int64 BufferSize;
		private Int64 ReceivedBytes;
		private Int64 TotalBytes;
		private readonly String URL;

		/// <summary>
		/// Creates a new <see cref="DownloadClient" /> that will
		/// obtain the file from <paramref name="URL" /> with
		/// <paramref name="BufferSize" /> bytes at a time
		/// </summary>
		/// <param name="URL">
		/// The URL where to download the content from
		/// </param>
		/// <param name="BufferSize">
		/// The amount of bytes to use in the buffer (default 1 KiB)
		/// </param>
		public DownloadClient ( String URL, Int64 BufferSize = 1048576 )
		{
			this.URL = URL;
			this.BufferSize = BufferSize;
		}

		/// <summary>
		/// Called when progress is made on the download
		/// </summary>
		public event DownloadClientDownloadProgressChangedHandler DownloadProgressChanged;

		/// <summary>
		/// Indicates whether this <see cref="DownloadClient" />
		/// is downloading
		/// </summary>
		public Boolean IsWorking { get; private set; }

		/// <summary>
		/// Downloads the resource on <see cref="URL" /> to <paramref name="path" />
		/// </summary>
		/// <param name="path">
		/// The path where to save the file to
		/// </param>
		/// <returns></returns>
		public async Task DownloadToFileAsync ( String path )
		{
			using ( FileStream fileStream = File.Open ( path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None ) )
				await this.DownloadToStreamAsync ( fileStream );
		}

		/// <summary>
		/// Download the contents of <see cref="URL" /> and
		/// returns them as an array of bytes
		/// </summary>
		/// <returns></returns>
		public async Task<Byte[]> DownloadBytesAsync ( )
		{
			using ( var memStream = new MemoryStream ( ) )
			{
				await this.DownloadToStreamAsync ( memStream );
				return memStream.ToArray ( );
			}
		}

		/// <summary>
		/// Downloads the contents of <see cref="URL" /> as a
		/// String with the possibility to provide an encoding to
		/// use to decode the bytes
		/// </summary>
		/// <param name="encoding">
		/// The encoding which to use when transforming from bytes
		/// to a string (Defaults to Default encoding)
		/// </param>
		/// <returns></returns>
		public async Task<String> DownloadStringAsync ( Encoding encoding = null )
		{
			encoding = encoding ?? Encoding.Default;
			return encoding.GetString ( await this.DownloadBytesAsync ( ) );
		}

		/// <summary>
		/// Downloads the resource from the provided URL and
		/// writes the contents of it as they are read.
		/// </summary>
		/// <param name="stream">The stream where to write to</param>
		/// <returns></returns>
		public async Task DownloadToStreamAsync ( Stream stream )
		{
			// Get the response for the contents of the file
			HttpWebResponse response = await this.GetResponseAsync ( );
			Int64 size = response.ContentLength;
			this.TotalBytes = size;

			using ( Stream webStream = response.GetResponseStream ( ) )
			{
				var buff = new Byte[this.BufferSize];
				while ( size > 0 )
				{
					var dl = ( Int32 ) Math.Min ( size, this.BufferSize );
					// Get number of bytes read as we can receive
					// less than we asked for
					dl = await webStream.ReadAsync ( buff, 0, dl );
					// Check for EOF
					if ( dl == 0 )
						break;
					// Update remaining byte count and received
					// byte count
					size -= dl;
					this.ReceivedBytes += dl;

					// Write from buffer to the stream and flush it
					await stream.WriteAsync ( buff, 0, dl );
					await stream.FlushAsync ( );

					// Then report that progress was made
					this.DownloadProgressChanged?.Invoke ( this, new DownloadClientDownloadProgressChangedArgs
					{
						BytesReceived = this.ReceivedBytes,
						TotalBytes = this.TotalBytes
					} );
				}
				// Indicate that the buffer is no longer being used
				buff = null;
			}
			// Attempt to force the GC into collecting the buffer
			GC.Collect ( );
		}

		/// <summary>
		/// Returns a response from the URL
		/// </summary>
		/// <returns></returns>
		private async Task<HttpWebResponse> GetResponseAsync ( )
		{
			HttpWebRequest req = WebRequest.CreateHttp ( this.URL );
			return ( HttpWebResponse ) await req.GetResponseAsync ( );
		}
	}
}
