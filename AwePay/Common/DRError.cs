using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Common
{
    public class DRError
    {
        public DRError()
        {

        }

        public string Type { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string InnerMsg { get; set; }
        public string Lang { get; set; }
        public List<string> Errors { get; set; }
    }
}
