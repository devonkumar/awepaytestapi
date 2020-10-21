using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Common
{
    public class PageQuery
    {
        public int PageNo { 
            get { return _PageNo;  }
            set { _PageNo = (value>0)?value:defPageNo; } 
        }
        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = (value <= defPageSize && value > 0) ? value : defPageSize; }
        }

        public string SortBy { get; set; } = "Id";
        public string SortDir { get; set; } = "asc";

        public string SearchCol { get; set; } = "";

        public string SearchBy { get; set; } = "";

        public int PageOff { get { return PageNo<=0?0:(PageNo - 1) * PageSize; } }
        public int ResCount { get; set; } = 0;

        private int defPageSize = 50;
        private int _PageSize = 50;
        private int _PageNo = 1;
        private int defPageNo = 1;



    }
}
