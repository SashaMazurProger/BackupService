using BackupClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBackup
{
    public class ConsoleAppClass
    {
        ControllerBackupBase controller;
        public ConsoleAppClass(ControllerBackupBase controller)
        {
            this.controller = controller;
        }
        public void ShowMainMenu()
        {
            Console.WriteLine("\n\t\tBACKUP SERVICE---MAIN MENU ");
            while (true)
            {
                Console.WriteLine("\n\nCHOOSE OPERATION:");
                Console.WriteLine("\n1-Show All objects");
                Console.WriteLine("\n2-Add backup object");
                Console.WriteLine("\n3-Delete object");
                Console.WriteLine("\n4-Exit");
                Console.Write("\n\nNumber of operation: ");
                int operation = int.Parse(Console.ReadLine());
                switch (operation)
                {
                    case 1: ShowAllObjects();
                        break;
                    case 2: AddObject();
                        break;
                    case 3:
                        DeleteObject();
                            break;
                    case 4:
                            return;
                    default:
                            Console.WriteLine("\nWrong symbol");
                            break;
                }
            }
        }
        public void ShowAllObjects()
        {
            List<BackupObject> objects = controller.GetBackupObjects().ToList();
            Console.Write("\n*************Backup Objects**************");
            if (!objects.Any())
            {
                Console.WriteLine("\n--Empty--");
                return;
            }
            for (int i = 0; i < objects.Count; ++i)
            {
                Console.Write("\n{0} \nName:{1} \nFrom:{2}  \nTo:{3}",
                    i,objects[i].Name, objects[i].FromPath,objects[i].ToPath);
            }
            Console.Write("\n*****************************************");
        }
        public void AddObject()
        {
         
            Console.Write("\nName:");
            string name = Console.ReadLine();
            Console.Write("\nFrom path:");
            string fromPath = Console.ReadLine();
            Console.Write("\nTo path:");
            string toPath = Console.ReadLine();
            try
            {
                controller.AddObjectBackup(name, fromPath, toPath);
            }
            catch (Exception e)
            {
                Console.WriteLine();
                Console.WriteLine(e.Message);
                AddObject();
            }
            
        }
        public void DeleteObject()
        {
            Console.Write("\nInput from path of deleted backup object: ");
            string fromPath = Console.ReadLine();
            BackupObject obj=controller.GetBackupObjects().First(o=>fromPath.Contains(fromPath));
            controller.DeleteObjectBackup(obj);
        }
    }
}
