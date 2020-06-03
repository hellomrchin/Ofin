using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models.Account
{
    public class AdminContext : DbContext
    {
        public AdminContext(DbContextOptions<AdminContext> options) : base(options)
        {
        }

        public DbSet<Admin> tAdmin { get; set; }
    }


    public class Admin
    {
        [Key]
        [DataMember(Name = "Username", IsRequired = true)]
        public string fUsername { get; set; }
        [DataMember(Name = "Password", IsRequired = true)]
        public string fPassword { get; set; }
        public string fAccessToken { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime fRegTime { get; set; }
        public string fIsVerified { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime fLastLogin { get; set; }
    }
}


