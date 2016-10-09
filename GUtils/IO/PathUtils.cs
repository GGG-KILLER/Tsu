using System;
using System.Runtime.InteropServices;
using System.Text;

namespace GUtils.IO
{
    internal class PathUtils
    {
        [DllImport ( "shlwapi.dll", EntryPoint = nameof ( PathRelativePathTo ) )]
        private static extern Boolean PathRelativePathTo ( StringBuilder lpszDst, String from, UInt32 attrFrom, String to, UInt32 attrTo );

		// All credit goes to http://stackoverflow.com/a/6194678/2671392
        /// <summary>
        /// Gets a path relative to another
        /// </summary>
        /// <param name="From">The path to start on</param>
        /// <param name="To">The path to go to</param>
        /// <returns>The relative path</returns>
        public static string GetRelativePath ( String From, String To )
        {
            var builder = new StringBuilder ( 1024 );
            var result = PathRelativePathTo ( builder, From, 0, To, 0 );
            return builder.ToString ( );
        }
    }
}