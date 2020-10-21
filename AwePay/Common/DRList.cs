using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Common
{
    public class DRList<T>
    {
        public List<T> Data { get; set; }

        public DRError Error { get; set; }

        public DRList(IEnumerable<T> collection)
        {
            Data = collection.ToList();
        }

        public DRList(List<T> collection)
        {
            Data = collection;
        }

    }


}
