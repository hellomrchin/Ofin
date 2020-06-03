using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models.eWallet.Topup
{
    public class BalanceContext : DbContext
    {
        public BalanceContext(DbContextOptions<BalanceContext> options) : base(options)
        {
        }

        public DbSet<Balances> tBalance { get; set; }
    }

    public class Balances
    {
        [Key]
        public string fUsername { get; set; }
        public int fBalance { get; set; }
        public DateTime fLastTopUp { get; set; }
    }
}
