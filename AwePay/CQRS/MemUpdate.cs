using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.CQRS
{
    public class MemUpdate
    {
        public int? Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string UName { get; set; }
        public string Paswd { get; set; }
        public string Trpin { get; set; }

        public bool En { get; set; }
        public long RId { get; set; }
        public long RefId { get; set; }
        public string RefUName { get; set; }
        public bool Chpass { get; set; }

        public bool Chtpin { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
