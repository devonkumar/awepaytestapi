using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Common
{
    public class DRSingle<T>
    {
        public T Data { get; set; }
        public DRError Error { get; set; }

        public DRSingle()
        {
            Error = new DRError();
        }
        public DRSingle(T single)
        {
            Data = single;
        }


        // public ActionResult GetActionResult(Func<EZSingle<T>, bool> criteria = null);
    }
}
