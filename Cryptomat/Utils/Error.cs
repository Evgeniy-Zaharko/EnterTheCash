using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptomat.Utils
{
    public class Error
    {
        public enum Code
        {
            Sucess,
            MissingConfigFile,
            InvalidJsonFormat,
            CantLaunchCashCode,
            CantDisableCashCode,
        }
    }
}
