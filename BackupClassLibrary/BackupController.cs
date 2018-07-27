using BackupClassLibrary.Abstract;
using BackupClassLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackupClassLibrary
{
    public class BackupController : ControllerBackupBase
    {
        IObjectRepository repository;
        
        public BackupController(IObjectRepository repo)
        {
            repository = repo;
        }
        

        public override void AddObjectBackup(string name,string fromPath, string toPath)
        {
            if ((Directory.Exists(fromPath) && Directory.Exists(toPath)) ||
                (File.Exists(fromPath) && Directory.Exists(toPath)))
            {
                BackupObject obj = new BackupObject
                {
                    Name = name,
                    FromPath = fromPath,
                    ToPath = toPath
                };
                repository.AddBackupObject(obj);
                base.InvokeAddedEvent(obj);
            }
            else
            {
                throw  new WrongPathException();
            }
        }

        public override void DeleteObjectBackup(BackupObject delete)
        {
                BackupObject obj = repository.GetList().First(o=>o.FromPath==delete.FromPath);
                if (obj != null)
                {
                    repository.DeleteObject(obj);
                    base.InvokeDeletedEvent(obj);
                }
        }
        public override void EditObjectBackup(string fromPath, string toPath)
        {
            if ((Directory.Exists(fromPath) && Directory.Exists(toPath)) ||
                (File.Exists(fromPath) && File.Exists(toPath)))
            {
                BackupObject obj = new BackupObject
                {
                    FromPath = fromPath,
                    ToPath = toPath
                };
                repository.Edit(obj);
                base.InvokeEditedEvent(obj);
            }
        }
        public override IEnumerable<BackupObject> GetBackupObjects()
        {
            return repository.GetList();
        }

        public override void StartBackupAll()
        {

            foreach (var obj in GetBackupObjects())
            {
                Thread thread;
       
                if (Directory.Exists(obj.FromPath))
                {
                   thread=new Thread (new ParameterizedThreadStart (new Backuper().CopyDirectory));
                   thread.Start(obj);
                }
                else if (File.Exists(obj.FromPath))
                {
                    thread = new Thread(new ParameterizedThreadStart(new Backuper().CopyFile));
                    thread.Start(obj);
                }
                
            }

         }
         public override void StartBackup(BackupObject obj)
         {
             Thread thread;

             if (Directory.Exists(obj.FromPath))
             {
                 thread = new Thread(new ParameterizedThreadStart(new Backuper().CopyDirectory));
                 thread.Start(obj);
             }
             else if (File.Exists(obj.FromPath))
             {
                 thread = new Thread(new ParameterizedThreadStart(new Backuper().CopyFile));
                 thread.Start(obj);
             }
         }
        
    }
}
