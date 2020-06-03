using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models.Account
{
    public class AdminDetailContext : DbContext
    {
        public AdminDetailContext(DbContextOptions<AdminDetailContext> options) : base(options)
        {
        }

        public DbSet<AdminDetail> tAdminDetail { get; set; }
    }

    public class AdminDetail
    {
        [Key]
        public string fUsername { get; set; }
        public string fIcNumber { get; set; }
        public string fFirstName { get; set; }
        public string fLastName { get; set; }
        public string fGender { get; set; }
    }
}
