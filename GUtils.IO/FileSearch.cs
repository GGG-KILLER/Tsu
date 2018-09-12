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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GUtils.IO
{
    public static class FileSearch
    {
        /// <summary>
        /// Internal tree class
        /// </summary>
        private class StringNode
        {
            private readonly IList<StringNode> Children;
            private readonly String Content;

            public StringNode ( String Content )
            {
                this.Content = Content;
                this.Children = new List<StringNode> ( );
            }

            public void AddChild ( StringNode Child )
            {
                this.Children.Add ( Child );
            }

            public String[] Flatten ( )
            {
                var @out = new List<String> ( );
                this.Flatten ( @out );
                return @out.ToArray ( );
            }

            public void Flatten ( IList<String> @out )
            {
                if ( this.Content != null )
                    @out.Add ( this.Content );

                foreach ( StringNode child in this.Children )
                    child.Flatten ( @out );
            }
        }

        /// <summary>
        /// Searchs for a file (globs not supported) while
        /// ignoring any exceptions that are caused by permission errors
        /// </summary>
        /// <param name="Path">Path to search in</param>
        /// <param name="FileName">Name of file to search</param>
        /// <returns></returns>
        public static String[] SafeSearch ( String Path, String FileName )
        {
            var Root = new StringNode ( null );
            SubSearch ( new DirectoryInfo ( Path ), FileName, Root );
            return Root.Flatten ( );
        }

        /// <summary>
        /// Searchs for a file (globs not supported)
        /// </summary>
        /// <param name="Dir">Directory to search in</param>
        /// <param name="Fn">Name of file to search</param>
        /// <param name="Parent">Parent node</param>
        /// <returns></returns>
        private static void SubSearch ( DirectoryInfo Dir, String Fn, StringNode Parent )
        {
            var node = new StringNode ( null );
            Parent.AddChild ( node );

            try
            {
                FileInfo[] fs = Dir.GetFiles ( Fn, SearchOption.TopDirectoryOnly );
                for ( var i = 0; i < fs.Length; i++ )
                    node.AddChild ( new StringNode ( fs[i].FullName ) );

                foreach ( DirectoryInfo dir in Dir.GetDirectories ( ) )
                    SubSearch ( dir, Fn, node );
            }
            catch ( Exception )
            {
                // We silently eat up errors
                return;
            }
        }
    }
}
