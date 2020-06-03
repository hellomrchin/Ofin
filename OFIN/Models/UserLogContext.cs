using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models
{
    public class UserLogContext : DbContext
    {
        public UserLogContext(DbContextOptions<UserLogContext> options) : base(options)
        {
        }

        public DbSet<UserLog> tUserLogs { get; set; }
    }

    public class UserLog
    {
        [Key]
        public string fLogId { get; set; }
        public string fUsername { get; set; }
        public string fAction { get; set; }
        public string fActionDesc { get; set; }
        public DateTime fDate { get; set; }

    }
}
