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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace GUtils.IO
{
    public static class FileSearch
    {
        /// <summary>
        /// Searchs for a file recursively (globs not supported)
        /// while ignoring any exceptions that are caused by
        /// permission errors
        /// </summary>
        /// <param name="root">Path to search in</param>
        /// <param name="searchPattern">Name of file to search</param>
        /// <returns></returns>
        public static String[] SafeSearch ( String root, String searchPattern ) =>
            SafeEnumerate ( root, searchPattern )
                .Select ( fi => fi.FullName )
                .ToArray ( );

        /// <summary>
        /// Performs a recursive search while ignoring any exceptions.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="searchPattern"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> SafeEnumerate ( String root, String searchPattern )
        {
            var directoryQueue = new Queue<DirectoryInfo> ( );
            directoryQueue.Enqueue ( new DirectoryInfo ( root ) );

            while ( directoryQueue.Count > 0 )
            {
                DirectoryInfo directoryInfo = directoryQueue.Dequeue ( );
                IEnumerable<FileInfo> files; IEnumerable<DirectoryInfo> directories;

                try
                {
                    files = directoryInfo.EnumerateFiles ( searchPattern, SearchOption.TopDirectoryOnly );
                    directories = directoryInfo.EnumerateDirectories ( );
                }
                catch ( Exception ex ) when ( ex is DirectoryNotFoundException || ex is SecurityException )
                {
                    continue;
                }

                foreach ( DirectoryInfo directory in directories )
                    directoryQueue.Enqueue ( directory );

                foreach ( FileInfo file in files )
                    yield return file;
            }
        }
    }
}
