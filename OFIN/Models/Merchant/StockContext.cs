using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models.Merchant
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }

        public DbSet<Stocks> tStock { get; set; }
    }

    public class Stocks
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string fItemCode { get; set; }
        public int fItemStock { get; set; }
        public DateTime fLastUpdate { get; set; }
    }
}
