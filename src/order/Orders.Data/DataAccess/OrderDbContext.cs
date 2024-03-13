using Microsoft.EntityFrameworkCore;
using Orders.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Data.DataAccess
{
    public class OrderDbContext : DbContext 
    {

        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            base.OnModelCreating(modelBuilder);
        }
        DbSet<Order> Orders { get; set; }

        DbSet<OrderDetail> OrderDetails { get; set; }

    }
}
