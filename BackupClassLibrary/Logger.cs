using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupClassLibrary
{
    public class Logger
    {
        private static object objLock = new object();
        public static void RecordMessageToLog(string message)
        {
            lock (objLock)
            {
                using (StreamWriter writer = new StreamWriter("C:\\backupLog.txt", true))
                {
                    writer.WriteLine(String.Format("{0} : {1}",
                        DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),message));
                    writer.Flush();
                }
            }
        }
        public static void RecordEventToLog(string name, string fileEvent, string filePath)
        {
            lock (objLock)
            {
                using (StreamWriter writer = new StreamWriter("C:\\backupLog.txt", true))
                {
                    writer.WriteLine(String.Format("{0} : {1}({2}) - {3}",
                        DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), fileEvent, name, filePath));
                    writer.Flush();
                }
            }
        }
    }
}
