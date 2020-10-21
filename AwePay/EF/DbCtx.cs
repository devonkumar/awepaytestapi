using AwePay.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwePay.EF
{
    public class DbCtx : DbContext
    {


        public DbCtx(DbContextOptions<DbCtx> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {


            base.OnModelCreating(builder);


            string password = "123456";
            byte[] passwordHash, passwordSalt;
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                builder.Entity<User>().HasData(
                    new User { Id = 1, FName = "Devon", LName = "Kumar", UName = "Admin", PHash = passwordHash, PSalt = passwordSalt, RoleCSV = "Admin" }
                    );


            }

        }

        public DbSet<User> Users { get; set; }

    }
}
