using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace WPCKillerApp
{
    internal static class Constants{
        public static string ProgramVersionBase
        {
            get
            {
                try
                {
                    var exePath = Process.GetCurrentProcess().MainModule?.FileName;
                    if (exePath == null)
                    {
                        return "";
                    }
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(exePath);
                    return $"v{fvi.FileVersion}";
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }
        public static string AppDataFolderName = "WPCKillerApp";
        public static string WindowsStartupFolder => Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public static string WindowsAppDataFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string AppDataFolder => Path.Combine(WindowsAppDataFolder, AppDataFolderName);
        public static string AppShortcutPath => Path.Join(WindowsStartupFolder, "WPCKillerApp.lnk");
        public static string? ExePath => Process.GetCurrentProcess().MainModule?.FileName;
    }
}
