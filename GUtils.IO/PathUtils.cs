using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GUtils.IO
{
    internal class PathUtils
    {
        [DllImport ( "shlwapi.dll" )]
        private static extern bool PathRelativePathTo (
             [Out] StringBuilder pszPath,
             [In] string pszFrom,
             [In] FileAttributes dwAttrFrom,
             [In] string pszTo,
             [In] FileAttributes dwAttrTo
        );

        /// <summary>
        /// Gets a path relative to another
        /// </summary>
        /// <param name="From">The path to start on</param>
        /// <param name="To">The path to go to</param>
        /// <returns>The relative path</returns>
        /// <remarks>
        /// Derived from
        /// http://pinvoke.net/default.aspx/shlwapi/PathRelativePathTo.html,
        /// and modified by me
        /// </remarks>
        public static string GetRelativePath ( FileSystemInfo From, FileSystemInfo To )
        {
            const Int32 MAX_PATH = 260;
            var str = new StringBuilder ( MAX_PATH );
            var result = PathRelativePathTo (
                str,
                From.FullName,
                From is DirectoryInfo ? FileAttributes.Directory : FileAttributes.Normal,
                To.FullName,
                To is DirectoryInfo ? FileAttributes.Directory : FileAttributes.Normal );

            return str.ToString ( );
        }
    }
}