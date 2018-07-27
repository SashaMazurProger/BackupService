using BackupClassLibrary;
using BackupClassLibrary.Abstract;
using BackupService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            IObjectRepository repo = new ObjectRepository();
            ControllerBackupBase con = new BackupController(repo);

            MainProcess mainProcess = new MainProcess(con);
            Thread backupThread = new Thread(new ThreadStart(mainProcess.Start));
            backupThread.Start();

            ConsoleAppClass main = new ConsoleAppClass(con);
            main.ShowMainMenu();

            Console.ReadKey();
        }
    }
}
