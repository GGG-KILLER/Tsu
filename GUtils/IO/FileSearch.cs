using System;
using System.IO;
using System.Linq;

namespace GUtils.IO
{
    public class FileSearch
    {
        /// <summary>
        /// Searchs for a file (globs not supported)
        /// </summary>
        /// <param name="Path">Path to search in</param>
        /// <param name="FileName">Name of file to search</param>
        /// <returns></returns>
        public static String[] SafeSearch ( String Path, String FileName )
        {
            return SafeSearch ( new DirectoryInfo ( Path ), FileName );
        }

        /// <summary>
        /// Searchs for a file (globs not supported)
        /// </summary>
        /// <param name="Dir">Directory to search in</param>
        /// <param name="Fn">Name of file to search</param>
        /// <returns></returns>
        public static String[] SafeSearch ( DirectoryInfo Dir, String Fn )
        {
            var fs = Dir.GetFiles ( Fn, SearchOption.TopDirectoryOnly );

            if ( fs.Length > 0 )
                return fs.Select ( f => f.FullName ).ToArray ( );

            foreach ( var dir in Dir.GetDirectories ( ) )
            {
                var r = SafeSearch ( dir, Fn );

                if ( r.Length > 0 )
                    return r;
            }

            return null;
        }
    }
}