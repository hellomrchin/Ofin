using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models.Merchant
{
    public class SupplierContext : DbContext
    {
        public SupplierContext(DbContextOptions<SupplierContext> options) : base(options)
        {
        }

        public DbSet<Suppliers> tSupplier { get; set; }
    }

    public class Suppliers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string fSupplierCode { get; set; }
        public string fSupplierName { get; set; }
        public string fSupplierEmail { get; set; }
        public string fSupplierPhone { get; set; }
        public string fRemark { get; set; }
        public string fCreatedBy { get; set; }
        public DateTime fCreatedDate { get; set; }
        public string fCreatedAt { get; set; }
        public string fModifiedBy { get; set; }
        public DateTime fModifiedDate { get; set; }
        public string fModifiedAt { get; set; }
    }
}
