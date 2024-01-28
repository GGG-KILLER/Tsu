// Copyright � 2021 GGG KILLER <gggkiller2@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software
// and associated documentation files (the �Software�), to deal in the Software without
// restriction, including without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom
// the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED �AS IS�, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System.Diagnostics;

namespace Tsu.Windows
{
    /// <summary>
    /// A static class with utilities to open files (not streams)
    /// in different ways
    /// </summary>
    public static class FExplorer
    {
        /// <summary>
        /// Opens explorer.exe with <paramref name="Path" /> as
        /// the working directory
        /// </summary>
        /// <param name="Path"></param>
        public static void OpenFolder(string Path) => Process.Start(new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = Path
        });

        /// <summary>
        /// Opens explorer.exe with <paramref name="Path" /> selected
        /// </summary>
        /// <param name="Path"></param>
        public static void OpenWithFileSelected(string Path) => Process.Start(new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = $"/e, /select, \"{Path}\""
        });

        /// <summary>
        /// Opens a file with the default program associated with it
        /// </summary>
        /// <param name="File"></param>
        public static void OpenWithDefaultProgram(string File) => Process.Start(File);

        /// <summary>
        /// Opens cmd.exe with the working directory set as <paramref name="Path" />
        /// </summary>
        /// <param name="Path"></param>
        public static void OpenFolderInCmd(string Path) => Process.Start(new ProcessStartInfo
        {
            WorkingDirectory = Path,
            FileName = "cmd.exe"
        });
    }
}
