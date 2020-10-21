using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.Domains
{
    [Table("User")]
    public class User
    {

        [Key, Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long UserId { get; set; }


        public bool En { get; set; }
        [StringLength(maximumLength: 255)]

        public string FName { get; set; }
        [StringLength(maximumLength: 255)]

        public string LName { get; set; }
        [StringLength(maximumLength: 255)]

        public string UName { get; set; }
        [StringLength(maximumLength: 255)]

        public byte[] PHash { get; set; }
        public byte[] PSalt { get; set; }
        [StringLength(maximumLength: 255)]
        public string LastIp { get; set; }

        [StringLength(maximumLength: 255)]
        public string TrPin { get; set; }

        [StringLength(maximumLength: 255)]

        public string Email { get; set; }
        [StringLength(maximumLength: 255)]

        public string Phone { get; set; }


        [NotMapped]
        public string Role { get; set; }
        [NotMapped]
        public string[] Roles { get; set; }
        private string _rolecsv;

        public string RoleCSV
        {
            get { return _rolecsv; }
            set
            {
                _rolecsv = value;
                Roles = value.Split(',');
            }
        }

    }
}
