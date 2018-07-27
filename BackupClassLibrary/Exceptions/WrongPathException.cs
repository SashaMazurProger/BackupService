using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackupClassLibrary.Exceptions
{
    public class WrongPathException:Exception
    {
        public override string Message
        {
            get
            {
                return "Directory or file don't exists";
            }
        }
    }
}
