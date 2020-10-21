using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Common
{
    public class DRCollection<T>
    {

        public List<T> Data { get; set; }
        public DRPaging Paging { get; set; }
        public DRError Error { get; set; }

        public DRCollection(IEnumerable<T> collection)
        {
            Data = collection.ToList();
        }

        public DRCollection(List<T> collection)
        {
            Data = collection;
        }
        public DRCollection(IEnumerable<T> collection, int pageNo, int pageSize, long resCount)
        {
            Data = collection.ToList();
            Paging = new DRPaging { PageNo = pageNo, PageSize = pageSize, ResCount = resCount, PageCount = (int)Math.Ceiling((decimal)resCount / pageSize) };
        }
        public DRCollection(List<T> collection, int pageNo, int pageSize, long resCount)
        {
            Data = collection;
            Paging = new DRPaging { PageNo = pageNo, PageSize = pageSize, ResCount = resCount, PageCount = (int)Math.Ceiling((decimal)resCount / pageSize) };

        }


        // public ActionResult GetActionResult(Func<ODCollection<T>, bool> criteria = null);
    }
}
