using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupClassLibrary.Exceptions
{
    public class LoadDataException:Exception
    {
        public LoadDataException(string message):base(message)
        {

        }
    }
}
