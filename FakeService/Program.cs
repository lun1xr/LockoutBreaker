using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FakeService
{
    internal static class Program
    {
        static void Main()
        {
            //InstallService();
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
    }
}

