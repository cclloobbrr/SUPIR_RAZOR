using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Models.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(o => o.Date)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(o => o.CustomerId)
                .IsRequired();

            builder
                .HasMany(o => o.ProductsOrders)
                .WithOne(po => po.Order)
                .HasForeignKey(o => o.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
