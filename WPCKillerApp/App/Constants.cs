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

        public static string WindowsStartupFolder => Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        public static string WindowsAppDataFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string AppShortcutPath => Path.Join(WindowsStartupFolder, "WPC Killer.lnk");
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
