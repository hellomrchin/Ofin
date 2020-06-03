using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models.Account
{
    public class UserDetailContext : DbContext
    {
        public UserDetailContext(DbContextOptions<UserDetailContext> options) : base(options)
        {
        }

        public DbSet<UserDetail> tUserDetail { get; set; }
    }

    public class UserDetail
    {
        [Key]
        public string fUsername { get; set; }
        public string fIcNumber { get; set; }
        public string fFirstName { get; set; }
        public string fLastName { get; set; }
        public string fGender { get; set; }
    }
}
