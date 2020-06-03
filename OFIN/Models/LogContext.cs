using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models
{
    public class LogContext : DbContext
    {
        public LogContext(DbContextOptions<LogContext> options) : base(options)
        {
        }

        public DbSet<Logs> tLogs { get; set; }
    }

    public class Logs
    {
        [Key]
        public string LogId { get; set; }
        public string LogGroup { get; set; }
        public string LogMsg { get; set; }
        public DateTime LogDate { get; set; }
        public string ErrorCode { get; set; }
        public string IpAddress { get; set; }
    }
}
