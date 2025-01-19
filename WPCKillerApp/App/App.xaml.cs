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
using System.Configuration;
using System.ServiceProcess;
using System.IO;
using System.Configuration.Install;
using static System.ServiceProcess.ServiceController;
using System.Security.Principal;

namespace Wpcmon.App
{
    public partial class App : Application
    {
        public TaskbarIcon _taskbarIcon = null!;
        private LaunchOpSettings _launchOpSettings = null!;
        private KeybindRegisterExecute _KeybindRegisterExecute = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _taskbarIcon = (TaskbarIcon)FindResource("TaskbarIcon");
            AdminCheck();
            _launchOpSettings = new LaunchOpSettings();
            if (ConfigurationManager.AppSettings["LaunchMinimized"] == "true")
            {
                
            }
            else
            {
                _launchOpSettings.Show();
            }
            //if (ConfigurationManager.AppSettings["SafeMode"] == "true")
            //{
            //  StartLeService();
            //}
            _KeybindRegisterExecute = new KeybindRegisterExecute();
            _KeybindRegisterExecute.Show();
            _KeybindRegisterExecute.Hide();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _taskbarIcon.Dispose();
            _launchOpSettings?.Close();
            _KeybindRegisterExecute.Close();
        }

        /*
        private static void StartLeService()
        {
            var processInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = "FakeService.exe",
            };
            try
            {
                Process.Start(processInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Service start failed: {ex.Message}");
            }
        }
        */
        private static void AdminCheck() 
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                var processInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = Environment.ProcessPath,
                    Verb = "runas"
                };

                try
                {
                    Process.Start(processInfo);
                    Application.Current.Shutdown();
                }
                catch (Exception)
                {
                    NoPerms noPerms = new();
                    noPerms.ShowDialog();
                }
            }
            else
            {

            }
        }
    }
}