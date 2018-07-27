using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace BackupService
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        ServiceInstaller installer;
        ServiceProcessInstaller process;
        public Installer1()
        {
            InitializeComponent();
            installer = new ServiceInstaller();
            process = new ServiceProcessInstaller();
            process.Account = ServiceAccount.LocalSystem;
            installer.StartType = ServiceStartMode.Manual;
            installer.ServiceName = "Service1";
            Installers.Add(process);
            Installers.Add(installer);
        }
    }
}
