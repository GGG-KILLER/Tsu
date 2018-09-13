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
/*
 * Copyright © 2016 GGG KILLER <gggkiller2@gmail.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”),
 * to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
 * IN THE SOFTWARE.
 */

namespace GUtils.IO
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public delegate void FileCopied ( String FileName, Int32 Processed, Int32 Total );

    public class FileCopier
    {
        public event FileCopied FileCopied;

        /// <summary>
        /// Copies multiple files asynchronously
        /// </summary>
        /// <param name="From">The array of files to copy</param>
        /// <param name="To">The array of files to paste</param>
        /// <param name="BufferSize">
        /// Size of buffer to use to copy data
        /// </param>
        /// <returns></returns>
        public async Task CopyFilesAsync ( IEnumerable<String> From, IEnumerable<String> To, Int32 BufferSize = 4096 )
        {
            if ( From.Count ( ) != To.Count ( ) )
                throw new ArgumentException ( "From length cannot be different than To's.", nameof ( From ) );

            var Len = From.Count ( );
            for ( var i = 0; i < Len; i++ )
            {
                await CopyFileAsync ( From.ElementAt ( i ), To.ElementAt ( i ), BufferSize );
                FileCopied?.Invoke ( From.ElementAt ( i ), i, Len );
            }
        }

        /// <summary>
        /// Copies a single file asynchronously
        /// </summary>
        /// <param name="From">The source file</param>
        /// <param name="To">The target file</param>
        /// <param name="BufferSize">
        /// The buffer size to use on the copy operation
        /// </param>
        /// <returns></returns>
        public async static Task CopyFileAsync ( String From, String To, Int32 BufferSize = 4096 )
        {
            if ( BufferSize < 1 )
                throw new ArgumentException ( $"Buffer size ({BufferSize}) cannot be smaller than 1", nameof ( BufferSize ) );

            var fi = new FileInfo ( From );
            var ti = new FileInfo ( To );
            if ( !File.Exists ( fi.FullName ) )
                throw new FileNotFoundException ( $"From file ({fi.FullName}) does not exists." );

            if ( ti.Exists )
                ti.Delete ( );
            ti.Directory.Create ( );
            using ( var reader = File.OpenRead ( fi.FullName ) )
            using ( var writer = File.OpenWrite ( ti.FullName ) )
            {
                var lastPos = 0L;
                while ( true )
                {
                    var count = ( Int32 ) ( reader.Length - reader.Position );
                    if ( count > BufferSize )
                        count = BufferSize;
                    Debug.Assert ( count > 0 );

                    lastPos = reader.Position;
                    var buffer = new Byte[count];

                    await reader.ReadAsync ( buffer, 0, count );
                    await writer.WriteAsync ( buffer, 0, count );

                    if ( count != BufferSize )
                        break;
                }
            }
        }
    }
}
