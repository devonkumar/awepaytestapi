using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Common
{
    public class DRQueRes<T>
    {


        public IQueryable<T> Query { get; set; }
        public IEnumerable<T> Result { get; set; }
        public long QueryCount;

        public DRQueRes(IQueryable<T> _query, IEnumerable<T> _result)
        {
            this.Query = _query;
            this.Result = _result;
            this.QueryCount = Query.Count();

        }
        public DRQueRes(IEnumerable<T> _result, int _count)
        {
            this.Result = _result;
            this.QueryCount = _count;
        }

        public static int GetOffset(int pageNo, int pageSize) => ((pageNo - 1) * pageSize);
    }



}
