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

namespace GUtils.IO
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Utility to copy a single file with a configurable buffer size
    /// </summary>
    public class FileCopier
    {
        /// <summary>
        /// Copies a single file asynchronously
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="target">The target file</param>
        /// <param name="bufferSize">
        /// The buffer size to use on the copy operation (4 KiB default)
        /// </param>
        /// <returns></returns>
        public async static Task CopyFileAsync ( String source, String target, Int32 bufferSize = 4096 )
        {
            if ( bufferSize < 1 )
                throw new ArgumentException ( $"Buffer size ({bufferSize}) cannot be smaller than 1", nameof ( bufferSize ) );

            var fi = new FileInfo ( source );
            var ti = new FileInfo ( target );
            if ( !fi.Exists )
                throw new FileNotFoundException ( $"From file ({fi.FullName}) does not exists." );

            // Delete the file if it already exists
            if ( ti.Exists )
                ti.Delete ( );
            // Create the full path up to the file's
            ti.Directory.Create ( );

            var buffer = new Byte[bufferSize];
            using ( FileStream reader = fi.OpenRead ( ) )
            using ( FileStream writer = ti.OpenWrite ( ) )
            {
                while ( reader.Position != reader.Length )
                {
                    var readBytes = await reader.ReadAsync ( buffer, 0, bufferSize ).ConfigureAwait ( false );
                    await writer.WriteAsync ( buffer, 0, readBytes ).ConfigureAwait ( false );
                }
            }
        }
    }
}
