using System;
using System.Diagnostics;

namespace GUtils.Windows
{
    public class FExplorer
    {
        public static void OpenFolder ( String Path )
        {
            Process.Start ( new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = Path
            } );
        }

        public static void OpenWithFileSelected ( String Path )
        {
            Process.Start ( new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"/e, /select, \"{Path}\""
            } );
        }

        public static void OpenWithDefaultProgram ( String File )
        {
            Process.Start ( File );
        }

        public static void OpenFolderInCmd ( String Path )
        {
            Process.Start ( new ProcessStartInfo
            {
                WorkingDirectory = Path,
                FileName = "cmd.exe"
            } );
        }
    }
}
