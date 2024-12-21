using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Wpcmon.App
{
    internal static class Constants
    {
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
        private static string GetProcessIdByExePath(string exePath)
        {
            try
            {
                if (!File.Exists(exePath))
                {
                    return "File not found";
                }
                var process = Process.GetProcesses()
                    .FirstOrDefault(predicate: p => p.MainModule?.FileName.Equals(exePath, StringComparison.OrdinalIgnoreCase) == true);

                return process != null ? process.Id.ToString() : "Process not found";
            }
            catch (Exception ex)
            {
                return $"{ex.Message}";
            }
        }
        public static void Filenotfound()
        {
            if (WPCid == "File not found")
            {
                MessageBox.Show("Wpcmon.exe not found. It has already been deleted or moved. Functions other than basic WPC killer will still work.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (WPCid == "Process not found")
            {
                MessageBox.Show("Unable to find process ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public static string WPCid => GetProcessIdByExePath(@"%systemroot%\System32\wpcmon.exe");
        public static string AppDataFolderName = "Wpcmon";
        public static string WindowsStartupFolder => Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public static string WindowsAppDataFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string AppDataFolder => Path.Combine(WindowsAppDataFolder, AppDataFolderName);
        public static string AppShortcutPath => Path.Join(WindowsStartupFolder, "Wpcmon.lnk");
        public static string? ExePath => Process.GetCurrentProcess().MainModule?.FileName;
        public static string? LocalFolder => Path.GetDirectoryName(ExePath);
        public static string IconPath => Path.Join(LocalFolder, "Res", "wpcmonxlight.ico");
        public static string LogoPath => Path.Join(LocalFolder, "Res", "wpcmonxdark.png");
    }
    public static class ConstantsWrapper
    {
        public static string IconPath => Constants.IconPath;

    }
}
