using BackupClassLibrary.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupClassLibrary
{
    public abstract class ControllerBackupBase
    {
        public event Action<BackupObject> Added;
        public event Action<BackupObject> Deleted;
        public event Action<BackupObject> Edited;
        public virtual void InvokeAddedEvent(BackupObject obj)
        {
            if(Added!=null)
            Added.Invoke(obj);
        }
        public virtual void InvokeEditedEvent(BackupObject obj)
        {
            if(Edited!=null)
            Edited.Invoke(obj);
        }
        public virtual void InvokeDeletedEvent(BackupObject obj)
        {
            if(Deleted!=null)
            Deleted.Invoke(obj);
        }
        public abstract void AddObjectBackup(string name,string fromPath,string toPath);
         public abstract void DeleteObjectBackup(BackupObject obj);
         public abstract void EditObjectBackup(string fromPath, string toPath);

        public abstract IEnumerable<BackupObject> GetBackupObjects();
        public abstract void StartBackupAll();
        public abstract void StartBackup(BackupObject obj);
    }
}
