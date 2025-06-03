using Microsoft.EntityFrameworkCore;
using SUPIR_RAZOR.Models.Configurations;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Data
{
    public class SUPIR_RAZORDbContext(DbContextOptions<SUPIR_RAZORDbContext> options) : DbContext(options)
    {
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<MasterEntity> Masters { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductOrderEntity> ProductsOrders { get; set; }
        public DbSet<SupplyEntity> Supplies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MasterConfiguration());
            modelBuilder.ApplyConfiguration(new SupplyConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductOrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
