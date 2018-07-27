using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using BackupClassLibrary;
using BackupClassLibrary.Abstract;

namespace BackupService
{
    public partial class Service1 : ServiceBase
    {
        MainProcess mainProcess;
        public Service1()
        {
            InitializeComponent();
            this.CanStop = true;
            this.CanPauseAndContinue = true;
            this.AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            
            IObjectRepository repository = new ObjectRepository();
            ControllerBackupBase controller=new BackupController(repository);
            mainProcess = new MainProcess(controller);
            Thread backupThread = new Thread(new ThreadStart(mainProcess.Start));
            backupThread.Start();
        }

        protected override void OnStop()
        {
            mainProcess.Stop();
            Thread.Sleep(2000);
        }
    }
}
