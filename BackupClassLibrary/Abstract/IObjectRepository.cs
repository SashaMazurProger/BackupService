using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupClassLibrary.Abstract
{
    public interface IObjectRepository
    {
        void AddBackupObject(BackupObject obj);
        IEnumerable<BackupObject> GetList();
        void DeleteObject(BackupObject obj);
        void Edit(BackupObject obj);
    }
}
