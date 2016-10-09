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
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.Extensions.FileSystemGlobbing;
    using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

    /// <summary>
    /// Glob searcher (accepts globstar "**" and star "*")
    /// </summary>
    public class GlobSearch
    {
        /// <summary>
        /// Exclude pattern list
        /// </summary>
        private List<String> ExcludePatterns = new List<String> ( );

        /// <summary>
        /// Include pattern list
        /// </summary>
        private List<String> IncludePatterns = new List<String> ( );

        /// <summary>
        /// Creates a Glob searcher (accepts globstar "**" and star "*")
        /// </summary>
        public GlobSearch ( ) { }

        /// <summary>
        /// Creates a Glob searcher (accepts globstar "**" and star "*")
        /// </summary>
        /// <param name="IncludePatterns">Patterns to use to find files</param>
        public GlobSearch ( IEnumerable<String> IncludePatterns )
        {
            this.IncludePatterns.AddRange ( IncludePatterns );
        }

        /// <summary>
        /// Creates a Glob searcher (accepts globstar "**" and star "*")
        /// </summary>
        /// <param name="IncludePatterns">Patterns to use to find files</param>
        /// <param name="ExcludePatterns">Patterns to use to exclude files</param>
        public GlobSearch ( IEnumerable<String> IncludePatterns, IEnumerable<String> ExcludePatterns )
        {
            this.IncludePatterns.AddRange ( IncludePatterns );
            this.ExcludePatterns.AddRange ( ExcludePatterns );
        }

        /// <summary>
        /// Adds a file finding pattern
        /// </summary>
        /// <param name="Pattern">Pattern to add</param>
        /// <returns></returns>
        public GlobSearch AddInclude ( String Pattern )
        {
            this.IncludePatterns.Add ( Pattern );
            return this;
        }

        /// <summary>
        /// Adds a file excluding pattern
        /// </summary>
        /// <param name="Pattern">Pattern to add</param>
        /// <returns></returns>
        public GlobSearch AddExclude ( String Pattern )
        {
            this.ExcludePatterns.Add ( Pattern );
            return this;
        }

        /// <summary>
        /// Adds a range of include patterns
        /// </summary>
        /// <param name="Patterns">Patterns to add</param>
        /// <returns></returns>
        public GlobSearch AddIncludePatterns ( IEnumerable<String> Patterns )
        {
            this.IncludePatterns.AddRange ( Patterns );
            return this;
        }

        /// <summary>
        /// Adds a range of exclude patterns
        /// </summary>
        /// <param name="Patterns">Patterns to add</param>
        /// <returns></returns>
        public GlobSearch AddExcludePatterns ( IEnumerable<String> Patterns )
        {
            this.ExcludePatterns.AddRange ( Patterns );
            return this;
        }

        /// <summary>
        /// Searches on the provided directory with the provided patterns
        /// </summary>
        /// <param name="Folder">The path of the folder to search on</param>
        /// <returns>The files found</returns>
        public IEnumerable<String> Search ( String Folder )
        {
            // Creates the directory info and runs the other function
            return Search ( new DirectoryInfo ( Folder ) );
        }

        /// <summary>
        /// Searches on the provided directory with the provided patterns
        /// </summary>
        /// <param name="Folder">The folder to search on</param>
        /// <returns></returns>
        public IEnumerable<String> Search ( DirectoryInfo Folder )
        {
            // Give out an error if the folder doesn't exists
            if ( !Folder.Exists )
                throw new DirectoryNotFoundException ( $"Couldn't find the directory {Folder}." );

            // Gives out an error if no patterns were provided
            if ( IncludePatterns.Count < 1 && ExcludePatterns.Count < 1 )
                throw new Exception ( "No include nor exclude patterns provided" );

            // Creates the matcher and adds the patterns
            var dir = new DirectoryInfoWrapper ( Folder );
            var matcher = new Matcher ( );
            matcher.AddIncludePatterns ( IncludePatterns );
            matcher.AddExcludePatterns ( ExcludePatterns );

            // Executes the matcher and returns the files
            return matcher.Execute ( dir )
                .Files
                .Select ( file => file.Path );
        }

        /// <summary>
        /// Transforms a Glob pattern into a Regex
        /// </summary>
        /// <param name="Pattern">The glob pattern</param>
        /// <returns>The Regex</returns>
        public static Regex GlobRegex ( String Pattern )
        {
            return new Regex (
                Regex.Escape ( Pattern )
                    .Replace ( @"\*\*", ".*" )
                    .Replace ( @"\*", @"[^\/\\]*" )
                    .Replace ( @"\?", "." )
                    .Replace ( "/", @"[\\/]" )
            );
        }
    }
}