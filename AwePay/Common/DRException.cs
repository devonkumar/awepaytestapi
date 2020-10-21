using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Common
{

[Serializable]
    public class DRException : Exception
    {
        public string Code = "";
        public new string Message = "";

        public DRException()
        {

        }

        public DRException(string code, string message) : base(String.Concat( code, ":", message))
        {
            Code = code;
            this.Message = message;
        }

    }
}
