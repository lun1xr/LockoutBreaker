using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FakeService
{
    internal static class Program
    {
        static void Main()
        {
            InstallService();
            UninstallBase();
        }

        private static void InstallService()
        {
            try
            {
                // Check if the event log source exists
                if (!EventLog.SourceExists("WpcMonSvcx"))
                {
                    // Create the event log source
                    EventLog.CreateEventSource("WpcMonSvcx", "Application");
                    Console.WriteLine("Event log source created successfully.");
                }

                using (AssemblyInstaller installer = new AssemblyInstaller(typeof(Installer1).Assembly, null))
                {
                    installer.UseNewContext = true;
                    installer.Install(null);
                    installer.Commit(null);
                }
                Console.WriteLine("Service installed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Service installation failed: {ex.Message}");
            }
        }
        private static void UninstallBase()
        {
            try
            {
                // Stop the service if it's running
                ServiceController service = new ServiceController("WpcMonSvc");
                if (service.Status != ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("Stopping the service...");
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    Console.WriteLine("Service stopped successfully.");
                }

                // Close the service controller
                service.Close();

                // Delete the service
                DeleteService("WpcMonSvc");

                Console.WriteLine("Service uninstalled successfully.");
            }
            catch (InvalidOperationException ex) when (ex.InnerException is System.ComponentModel.Win32Exception win32Ex &&
                                                       win32Ex.NativeErrorCode == 1060)
            {
                Console.WriteLine("Service does not exist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Service uninstallation failed: {ex.Message}");
            }

            try
            {
                // Delete the event log source
                if (EventLog.SourceExists("WpcMonSvc"))
                {
                    EventLog.DeleteEventSource("WpcMonSvc");
                    Console.WriteLine("Event log source deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Event log source does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Event log source deletion failed: {ex.Message}");
            }
        }

        private static void DeleteService(string serviceName)
        {
            using (ServiceController sc = new ServiceController(serviceName))
            {
                // Open service control manager
                IntPtr scmHandle = OpenSCManager(null, null, SC_MANAGER_ALL_ACCESS);
                if (scmHandle == IntPtr.Zero)
                {
                    throw new Exception("Failed to open service control manager.");
                }

                // Open the service
                IntPtr serviceHandle = OpenService(scmHandle, serviceName, SERVICE_ALL_ACCESS);
                if (serviceHandle == IntPtr.Zero)
                {
                    CloseServiceHandle(scmHandle);
                    throw new Exception("Failed to open service.");
                }

                // Delete the service
                if (!DeleteService(serviceHandle))
                {
                    CloseServiceHandle(serviceHandle);
                    CloseServiceHandle(scmHandle);
                    throw new Exception("Failed to delete service.");
                }

                // Close handles
                CloseServiceHandle(serviceHandle);
                CloseServiceHandle(scmHandle);
            }
        }

        // P/Invoke declarations
        private const int SC_MANAGER_ALL_ACCESS = 0xF003F;
        private const int SERVICE_ALL_ACCESS = 0xF01FF;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern IntPtr OpenSCManager(
            string lpMachineName,
            string lpDatabaseName,
            int dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern IntPtr OpenService(
            IntPtr hSCManager,
            string lpServiceName,
            int dwDesiredAccess);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool DeleteService(IntPtr hService);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CloseServiceHandle(IntPtr hSCObject);
    }
}

