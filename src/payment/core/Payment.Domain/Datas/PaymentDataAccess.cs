using Microsoft.EntityFrameworkCore;
using Payment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain.Datas
{
    public class PaymentDataAccess : DbContext
    {
        public DbSet<Merchant> Merchants { get; set; }


        public PaymentDataAccess(DbContextOptions<PaymentDataAccess> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}
