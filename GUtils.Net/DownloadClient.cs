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
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GUtils.Net
{
    /// <summary>
    /// The arguments to the progress changed event
    /// </summary>
    public struct DownloadClientDownloadProgressChangedArgs : IEquatable<DownloadClientDownloadProgressChangedArgs>
    {
        /// <summary>
        /// The amount of bytes already downloaded
        /// </summary>
        public Int64 BytesReceived { get; set; }

        /// <summary>
        /// The total amount of bytes to be downloaded
        /// </summary>
        public Int64 TotalBytes { get; set; }

        #region Generated Code

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override Boolean Equals ( Object obj ) =>
            obj is DownloadClientDownloadProgressChangedArgs && this.Equals ( ( DownloadClientDownloadProgressChangedArgs ) obj );

        /// <summary>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Boolean Equals ( DownloadClientDownloadProgressChangedArgs other ) =>
            this.BytesReceived == other.BytesReceived
            && this.TotalBytes == other.TotalBytes;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode ( )
        {
            var hashCode = 637993755;
            hashCode = hashCode * -1521134295 + this.BytesReceived.GetHashCode ( );
            hashCode = hashCode * -1521134295 + this.TotalBytes.GetHashCode ( );
            return hashCode;
        }

        /// <summary>
        /// </summary>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        /// <returns></returns>
        public static Boolean operator == ( DownloadClientDownloadProgressChangedArgs args1, DownloadClientDownloadProgressChangedArgs args2 ) => args1.Equals ( args2 );

        /// <summary>
        /// </summary>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        /// <returns></returns>
        public static Boolean operator != ( DownloadClientDownloadProgressChangedArgs args1, DownloadClientDownloadProgressChangedArgs args2 ) => !( args1 == args2 );

        #endregion Generated Code
    }

    /// <summary>
    /// An experimental download client meant to be faster than <see cref="WebClient"/>
    /// </summary>
    public class DownloadClient
    {
        /// <summary>
        /// The user agent to use when downloading the file
        /// </summary>
        public String UserAgent { get; set; }

        private readonly Int32 _bufferSize;
        private Int64 _receivedBytes;
        private Int64 _totalBytes;
        private readonly String _url;
        private readonly Uri _uri;

        /// <summary>
        /// Creates a new <see cref="DownloadClient"/> that will obtain the file from <paramref
        /// name="url"/> with <paramref name="bufferSize"/> bytes at a time.
        /// </summary>
        /// <param name="url">The URL where to download the content from</param>
        /// <param name="bufferSize">
        /// The amount of bytes to use in the buffer (values larger than 85000 will end up in the
        /// large object heap)
        /// </param>
        public DownloadClient ( String url, Int32 bufferSize = 16384 )
        {
            this._url = url;
            this._bufferSize = bufferSize;
        }

        /// <summary>
        /// Creates a new <see cref="DownloadClient"/> that will obtain the file from a <paramref
        /// name="uri"/> with <paramref name="bufferSize"/> bytes at a time.
        /// </summary>
        /// <param name="uri">The source URI</param>
        /// <param name="bufferSize">The size of the buffer</param>
        public DownloadClient ( Uri uri, Int32 bufferSize = 16384 )
        {
            this._uri = uri;
            this._bufferSize = bufferSize;
        }

        /// <summary>
        /// Called when progress is made on the download
        /// </summary>
        public event EventHandler<DownloadClientDownloadProgressChangedArgs> DownloadProgressChanged;

        /// <summary>
        /// Indicates whether this <see cref="DownloadClient"/> is downloading
        /// </summary>
        public Boolean IsWorking { get; private set; }

        /// <summary>
        /// Download the contents of <see cref="_url"/> and returns them as an array of bytes
        /// </summary>
        /// <returns></returns>
        public async Task<Byte[]> DownloadBytesAsync ( )
        {
            using var memStream = new MemoryStream ( );
            await this.DownloadToStreamAsync ( memStream )
                      .ConfigureAwait ( false );
            return memStream.ToArray ( );
        }

        /// <summary>
        /// Downloads the contents of <see cref="_url"/> as a String with the possibility to provide
        /// an encoding to use to decode the bytes
        /// </summary>
        /// <param name="encoding">
        /// The encoding which to use when transforming from bytes to a string (Defaults to Default encoding)
        /// </param>
        /// <returns></returns>
        public async Task<String> DownloadStringAsync ( Encoding encoding )
        {
            if ( encoding is null )
                throw new ArgumentNullException ( nameof ( encoding ) );
            return encoding.GetString ( await this.DownloadBytesAsync ( )
                                                  .ConfigureAwait ( false ) );
        }

        /// <summary>
        /// Downloads the resource on <see cref="_url"/> to <paramref name="path"/>
        /// </summary>
        /// <param name="path">The path where to save the file to</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task DownloadToFileAsync ( String path, Int32 timeout = 5000 )
        {
            using FileStream fileStream = File.Open ( path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read );
            await this.DownloadToStreamAsync ( fileStream, timeout )
                      .ConfigureAwait ( false );
        }

        /// <summary>
        /// Downloads the resource from the provided URL and writes the contents of it as they are read.
        /// </summary>
        /// <param name="stream">The stream where to write to</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task DownloadToStreamAsync ( Stream stream, Int32 timeout = 5000 )
        {
            // Get the response for the contents of the file
            HttpWebResponse response = await this.GetResponseAsync( )
                                                 .ConfigureAwait( false );

            var size = response.ContentLength;
            this._totalBytes = size;

            using Stream webStream = response.GetResponseStream ( );
            var buff = new Byte[this._bufferSize];
            while ( size != 0 )
            {
                Int32 receivedBytes;
                if ( timeout != -1 )
                {
                    using var source = new CancellationTokenSource ( timeout );
                    try
                    {
                        receivedBytes = await webStream.ReadAsync ( buff, 0, this._bufferSize, source.Token )
                                                       .ConfigureAwait ( false );
                    }
                    catch ( TaskCanceledException )
                    {
                        throw new TimeoutException ( "Reading operation timed out." );
                    }
                }
                else
                {
                    receivedBytes = await webStream.ReadAsync ( buff, 0, this._bufferSize )
                                                   .ConfigureAwait ( false );
                }

                // Check for EOF
                if ( receivedBytes == 0 )
                    size = 0;

                // Update remaining byte count and received byte count
                size -= receivedBytes;
                this._receivedBytes += receivedBytes;

                // Write from buffer to the stream and flush it
                await stream.WriteAsync ( buff, 0, receivedBytes )
                            .ConfigureAwait ( false );

                // Then report that progress was made
                this.DownloadProgressChanged?.Invoke ( this, new DownloadClientDownloadProgressChangedArgs
                {
                    BytesReceived = this._receivedBytes,
                    TotalBytes = this._totalBytes
                } );
            }

            // Remove possible sensitive information from the buffer
            Array.Clear ( buff, 0, buff.Length );
        }

        /// <summary>
        /// Returns a response from the URL
        /// </summary>
        /// <returns></returns>
        private async Task<HttpWebResponse> GetResponseAsync ( )
        {
            HttpWebRequest req = this._uri is null
                ? WebRequest.CreateHttp ( this._url )
                : WebRequest.CreateHttp ( this._uri );
            if ( this.UserAgent != null )
                req.UserAgent = this.UserAgent;

            return ( HttpWebResponse ) await req.GetResponseAsync ( )
                                                .ConfigureAwait ( false );
        }
    }
}