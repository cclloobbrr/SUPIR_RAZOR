using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Models.Configurations
{
    public class ProductOrderConfiguration : IEntityTypeConfiguration<ProductOrderEntity>
    {
        public void Configure(EntityTypeBuilder<ProductOrderEntity> builder)
        {
            builder.HasKey(po => new { po.OrderId, po.ProductId });

            builder.Property(po => po.OrderId)
                .IsRequired();

            builder.Property(po => po.ProductId)
                .IsRequired();

            builder.Property(po => po.Quantity)
                .IsRequired()
                .HasDefaultValue(0)
                .HasComment("Количество товара в продукте.");

            builder
                .HasOne(po => po.Product)
                .WithMany(p => p.ProductsOrders)
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(po => po.Order)
                .WithMany(o => o.ProductsOrders)
                .HasForeignKey(po => po.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
