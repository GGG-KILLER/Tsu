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