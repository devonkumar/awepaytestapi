using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Common
{
    public class DRPaging
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public long ResCount { get; set; }
    }
}
