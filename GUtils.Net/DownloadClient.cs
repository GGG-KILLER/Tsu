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
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GUtils.Net
{
    public struct DownloadClientDownloadProgressChangedArgs
    {
        public Int64 BytesReceived;
        public Int64 TotalBytes;
    }

    public class DownloadClient
    {
        /// <summary>
        /// The user agent to use when downloading the file
        /// </summary>
        public String UserAgent;

        private readonly Int32 BufferSize;
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
        /// The amount of bytes to use in the buffer (values
        /// larger than 85000 will end up in the large object heap)
        /// </param>
        public DownloadClient ( String URL, Int32 BufferSize = 16384 )
        {
            this.URL = URL;
            this.BufferSize = BufferSize;
        }

        /// <summary>
        /// Called when progress is made on the download
        /// </summary>
        public event EventHandler<DownloadClientDownloadProgressChangedArgs> DownloadProgressChanged;

        /// <summary>
        /// Indicates whether this <see cref="DownloadClient" />
        /// is downloading
        /// </summary>
        public Boolean IsWorking { get; private set; }

        /// <summary>
        /// Download the contents of <see cref="URL" /> and
        /// returns them as an array of bytes
        /// </summary>
        /// <returns></returns>
        public async Task<Byte[]> DownloadBytesAsync ( )
        {
            using ( var memStream = new MemoryStream ( ) )
            {
                await this.DownloadToStreamAsync ( memStream )
                    .ConfigureAwait ( false );
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
        public async Task<String> DownloadStringAsync ( Encoding encoding ) => encoding.GetString ( await this.DownloadBytesAsync ( )
                .ConfigureAwait ( false ) );

        /// <summary>
        /// Downloads the resource on <see cref="URL" /> to <paramref name="path" />
        /// </summary>
        /// <param name="path">
        /// The path where to save the file to
        /// </param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task DownloadToFileAsync ( String path, Int32 timeout = 5000 )
        {
            using ( FileStream fileStream = File.Open ( path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read ) )
            {
                await this.DownloadToStreamAsync ( fileStream, timeout )
                    .ConfigureAwait ( false );
            }
        }

        /// <summary>
        /// Downloads the resource from the provided URL and
        /// writes the contents of it as they are read.
        /// </summary>
        /// <param name="stream">The stream where to write to</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task DownloadToStreamAsync ( Stream stream, Int32 timeout = 5000 )
        {
            // Get the response for the contents of the file
            HttpWebResponse response = await this.GetResponseAsync ( )
                .ConfigureAwait ( false );

            var size = response.ContentLength;
            this.TotalBytes = size;

            using ( Stream webStream = response.GetResponseStream ( ) )
            {
                var buff = new Byte[this.BufferSize];
                while ( size != 0 )
                {
                    Int32 receivedBytes;
                    if ( timeout != -1 )
                    {
                        using ( var source = new CancellationTokenSource ( timeout ) )
                        {
                            try
                            {
                                receivedBytes = await webStream.ReadAsync ( buff, 0, this.BufferSize, source.Token ).ConfigureAwait ( false );
                            }
                            catch ( TaskCanceledException )
                            {
                                throw new TimeoutException ( "Reading operation timed out." );
                            }
                        }
                    }
                    else
                        receivedBytes = await webStream.ReadAsync ( buff, 0, this.BufferSize ).ConfigureAwait ( false );

                    // Check for EOF
                    if ( receivedBytes == 0 )
                        size = 0;

                    // Update remaining byte count and received
                    // byte count
                    size -= receivedBytes;
                    this.ReceivedBytes += receivedBytes;

                    // Write from buffer to the stream and flush it
                    await stream.WriteAsync ( buff, 0, receivedBytes ).ConfigureAwait ( false );

                    // Then report that progress was made
                    this.DownloadProgressChanged?.Invoke ( this, new DownloadClientDownloadProgressChangedArgs
                    {
                        BytesReceived = this.ReceivedBytes,
                        TotalBytes = this.TotalBytes
                    } );
                }

                // Remove possible sensitive information from the buffer
                for ( var i = 0; i < this.BufferSize; i++ )
                    buff[i] = 0;
            }
        }

        /// <summary>
        /// Returns a response from the URL
        /// </summary>
        /// <returns></returns>
        private async Task<HttpWebResponse> GetResponseAsync ( )
        {
            HttpWebRequest req = WebRequest.CreateHttp ( this.URL );
            if ( this.UserAgent != null )
                req.UserAgent = this.UserAgent;

            return ( HttpWebResponse ) await req.GetResponseAsync ( ).ConfigureAwait ( false );
        }
    }
}
