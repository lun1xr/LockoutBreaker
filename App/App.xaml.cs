using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.VisualBasic;

namespace WPCKillerApp.App
{
    public partial class App : Application
    {
        private TaskbarIcon _taskbarIcon = null!;
        private LaunchOpSettings _launchOpSettings = null!;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _taskbarIcon = (TaskbarIcon)FindResource("TaskbarIcon");
            _launchOpSettings = new LaunchOpSettings();
            _launchOpSettings.Show();
        }
        private void OnExit(object sender, ExitEventArgs e)
        {
            _taskbarIcon.Dispose();
            _launchOpSettings.Close();
        }
    }
}