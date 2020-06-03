using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OFIN.Models.Payment
{
    public class PaymentContext : DbContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
        }

        public DbSet<Payment> tPayment { get; set; }
    }

    public class Payment
    {
        [Key]
        public string fPaymentId { get; set; }
        public string fUsername { get; set; }
        public string fItemCode { get; set; }
        public int fQty { get; set; }
        public DateTime fPaymentDate { get; set; }
    }
}
