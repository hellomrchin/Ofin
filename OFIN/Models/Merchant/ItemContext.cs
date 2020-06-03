using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models.Merchant
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options) : base(options)
        {
        }

        public DbSet<Item> tItem { get; set; }
    }

    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string fItemCode { get; set; }
        public string fItemTitle { get; set; }
        public string fItemDesc { get; set; }
        public string fSupplier { get; set; }
        public string fItemPrice { get; set; }
        public int fItemBought { get; set; }
        public string fCreatedBy { get; set; }
        public DateTime fCreatedDate { get; set; }
        public string fCreatedAt { get; set; }
        public string fModifiedBy { get; set; }
        public DateTime fModifiedDate { get; set; }
        public string fModifiedAt { get; set; }
    }
}
