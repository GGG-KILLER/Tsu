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
#if HAS_SPAN
using System.Buffers;
#endif
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tsu.Net
{
    /// <summary>
    /// The arguments to the progress changed event
    /// </summary>
    public struct DownloadClientDownloadProgressChangedArgs : IEquatable<DownloadClientDownloadProgressChangedArgs>
    {
        /// <summary>
        /// The amount of bytes already downloaded
        /// </summary>
        public long BytesReceived { get; set; }

        /// <summary>
        /// The total amount of bytes to be downloaded
        /// </summary>
        public long TotalBytes { get; set; }

        #region Generated Code

        /// <summary>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj) =>
            obj is DownloadClientDownloadProgressChangedArgs args && Equals(args);

        /// <summary>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DownloadClientDownloadProgressChangedArgs other) =>
            BytesReceived == other.BytesReceived
            && TotalBytes == other.TotalBytes;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("Style", "IDE0070:Use 'System.HashCode'", Justification = "Not available on all target frameworks.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Applicable to some target frameworks.")]
        public override int GetHashCode()
        {
            var hashCode = 637993755;
            hashCode = hashCode * -1521134295 + BytesReceived.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalBytes.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// </summary>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        /// <returns></returns>
        public static bool operator ==(DownloadClientDownloadProgressChangedArgs args1, DownloadClientDownloadProgressChangedArgs args2) => args1.Equals(args2);

        /// <summary>
        /// </summary>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        /// <returns></returns>
        public static bool operator !=(DownloadClientDownloadProgressChangedArgs args1, DownloadClientDownloadProgressChangedArgs args2) => !(args1 == args2);

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
        public string? UserAgent { get; set; }

        private readonly int _bufferSize;
        private long _receivedBytes;
        private long _totalBytes;
        private readonly string? _url;
        private readonly Uri? _uri;

        /// <summary>
        /// Creates a new <see cref="DownloadClient"/> that will obtain the file from <paramref
        /// name="url"/> with <paramref name="bufferSize"/> bytes at a time.
        /// </summary>
        /// <param name="url">The URL where to download the content from</param>
        /// <param name="bufferSize">
        /// The amount of bytes to use in the buffer (values larger than 85000 will end up in the
        /// large object heap)
        /// </param>
        public DownloadClient(string url, int bufferSize = 16384)
        {
            _url = url;
            _bufferSize = bufferSize;
        }

        /// <summary>
        /// Creates a new <see cref="DownloadClient"/> that will obtain the file from a <paramref
        /// name="uri"/> with <paramref name="bufferSize"/> bytes at a time.
        /// </summary>
        /// <param name="uri">The source URI</param>
        /// <param name="bufferSize">The size of the buffer</param>
        public DownloadClient(Uri uri, int bufferSize = 16384)
        {
            _uri = uri;
            _bufferSize = bufferSize;
        }

        /// <summary>
        /// Called when progress is made on the download
        /// </summary>
        public event EventHandler<DownloadClientDownloadProgressChangedArgs>? DownloadProgressChanged;

        /// <summary>
        /// Indicates whether this <see cref="DownloadClient"/> is downloading
        /// </summary>
        public bool IsWorking { get; private set; }

        /// <summary>
        /// Download the contents of <see cref="_url"/> and returns them as an array of bytes
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> DownloadBytesAsync()
        {
            using var memStream = new MemoryStream();
            await DownloadToStreamAsync(memStream).ConfigureAwait(false);
            return memStream.ToArray();
        }

        /// <summary>
        /// Downloads the contents of <see cref="_url"/> as a String with the possibility to provide
        /// an encoding to use to decode the bytes
        /// </summary>
        /// <param name="encoding">
        /// The encoding which to use when transforming from bytes to a string (Defaults to Default encoding)
        /// </param>
        /// <returns></returns>
        public async Task<string> DownloadStringAsync(Encoding encoding)
        {
            if (encoding is null)
                throw new ArgumentNullException(nameof(encoding));
            return encoding.GetString(await DownloadBytesAsync().ConfigureAwait(false));
        }

        /// <summary>
        /// Downloads the resource on <see cref="_url"/> to <paramref name="path"/>
        /// </summary>
        /// <param name="path">The path where to save the file to</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task DownloadToFileAsync(string path, int timeout = 5000)
        {
            using var fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
            await DownloadToStreamAsync(fileStream, timeout).ConfigureAwait(false);
        }

        /// <summary>
        /// Downloads the resource from the provided URL and writes the contents of it as they are read.
        /// </summary>
        /// <param name="stream">The stream where to write to</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task DownloadToStreamAsync(Stream stream, int timeout = 5000)
        {
            if (stream is null)
                throw new ArgumentNullException(nameof(stream));

            // Get the response for the contents of the file
            var response = await GetResponseAsync().ConfigureAwait(false);

            var size = response.ContentLength;
            _totalBytes = size;

            using var webStream = response.GetResponseStream();
#if HAS_SPAN
            using var bufferOwner = MemoryPool<byte>.Shared.Rent(_bufferSize);
#else
            var buff = new byte[_bufferSize];
#endif
            while (size != 0)
            {
                int receivedBytes;
                if (timeout != -1)
                {
                    using var source = new CancellationTokenSource(timeout);
                    try
                    {
#if HAS_SPAN
                        receivedBytes = await webStream.ReadAsync(bufferOwner.Memory, source.Token)
#else
                        receivedBytes = await webStream.ReadAsync(buff, 0, _bufferSize, source.Token)
#endif
                                                       .ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        throw new TimeoutException("Read operation timed out.");
                    }
                }
                else
                {
#if HAS_SPAN
                    receivedBytes = await webStream.ReadAsync(bufferOwner.Memory)
#else
                    receivedBytes = await webStream.ReadAsync(buff, 0, _bufferSize)
#endif
                                                   .ConfigureAwait(false);
                }

                // Check for EOF
                if (receivedBytes == 0)
                    size = 0;

                // Update remaining byte count and received byte count
                size -= receivedBytes;
                _receivedBytes += receivedBytes;

                // Write from buffer to the stream
#if HAS_SPAN
                await stream.WriteAsync(bufferOwner.Memory.Slice(0, receivedBytes))
#else
                await stream.WriteAsync(buff, 0, receivedBytes)
#endif
                            .ConfigureAwait(false);

                // Then report that progress was made
                DownloadProgressChanged?.Invoke(this, new DownloadClientDownloadProgressChangedArgs
                {
                    BytesReceived = _receivedBytes,
                    TotalBytes = _totalBytes
                });
            }

            // Remove possible sensitive information from the buffer
#if HAS_SPAN
            bufferOwner.Memory.Span.Clear();
#else
            Array.Clear(buff, 0, buff.Length);
#endif
        }

        /// <summary>
        /// Returns a response from the URL
        /// </summary>
        /// <returns></returns>
        private async Task<HttpWebResponse> GetResponseAsync()
        {
            var req = _uri is null
#pragma warning disable IDE0079 // Remove unnecessary suppression (warning happens on some target frameworks)
#pragma warning disable CA2234 // Pass system uri objects instead of strings (it's being done below)
                ? WebRequest.CreateHttp(_url!)
#pragma warning restore CA2234 // Pass system uri objects instead of strings (it's being done below)
#pragma warning restore IDE0079 // Remove unnecessary suppression (warning happens on some target frameworks)
                : WebRequest.CreateHttp(_uri);
            if (UserAgent != null)
                req.UserAgent = UserAgent;

            return (HttpWebResponse) await req.GetResponseAsync()
                                              .ConfigureAwait(false);
        }
    }
}