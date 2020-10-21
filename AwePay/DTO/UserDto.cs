using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.DTO
{
    public class UserDto
    {
        public int? Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string UName { get; set; }
        public string Paswd { get; set; }
        public string TrPin { get; set; }

        public bool En { get; set; }
        public bool Chpass { get; set; }
        public bool Chtpin { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
