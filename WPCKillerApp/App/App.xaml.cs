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

namespace WPCKillerApp.App
{
    public partial class App : Application
    {
        public TaskbarIcon _taskbarIcon = null!;
        private LaunchOpSettings _launchOpSettings = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _taskbarIcon = (TaskbarIcon)FindResource("TaskbarIcon");
            _launchOpSettings = new LaunchOpSettings();
            _launchOpSettings.Show();
            if (ConfigurationManager.AppSettings["SafeMode"] == "true")
            {
                // Install the service if not already installed
                InstallService();

                // Start the service
                StartMyService();
            }
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            _taskbarIcon.Dispose();
            _launchOpSettings.Close();
        }

        private static void InstallService()
        {
            try
            {
                string serviceAssemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FakeService.exe");
                using AssemblyInstaller installer = new(serviceAssemblyPath, null);
                installer.UseNewContext = true;
                installer.Install(null);
                installer.Commit(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Service installation failed: {ex.Message}");
            }
        }

        private static void StartMyService()
        {
            try
            {
                ServiceController service = new("WpcMonSvcx");
                if (service.Status != ServiceControllerStatus.Running)
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Service start failed: {ex.Message}");
            }
        }
    }
}