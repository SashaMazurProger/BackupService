using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupClassLibrary.Exceptions
{
    public class SaveDataException :Exception
    {
        public SaveDataException(string message):base(message)
        {

        }
    }
}
