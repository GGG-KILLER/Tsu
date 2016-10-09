namespace GUtils.IO
{
    using System;
    using System.Collections.Generic;
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
        /// <param name="BufferSize">Size of buffer to use to copy data</param>
        /// <returns></returns>
        public async Task CopyFilesAsync ( IEnumerable<String> From, IEnumerable<String> To, Int32 BufferSize = 100 )
        {
            if ( From.Count ( ) != To.Count ( ) )
                throw new ArgumentException ( "From length cannot be different than To's.", nameof ( From ) );

            var Len = From.Count ( );
            for ( var i = 0 ; i < Len ; i++ )
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
        /// <param name="BufferSize">The buffer size to use on the copy operation</param>
        /// <returns></returns>
        public async static Task CopyFileAsync ( String From, String To, Int32 BufferSize = 100 )
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
                do
                {
                    var count = ( Int32 ) ( reader.Length - reader.Position );
                    lastPos = reader.Position;

                    if ( count < 0 || count > BufferSize )
                        count = BufferSize;

                    var buffer = new Byte[count];
                    await reader.ReadAsync ( buffer, 0, count );
                    await writer.WriteAsync ( buffer, 0, count );
                }
                while ( lastPos != reader.Position && reader.Position < reader.Length );
            }
        }
    }
}