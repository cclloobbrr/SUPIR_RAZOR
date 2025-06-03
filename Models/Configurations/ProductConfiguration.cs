using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Models.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.Material)
                .IsRequired()
                .HasMaxLength(150)
                .HasDefaultValue("Не определено");

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(p => p.Quantity)
                .IsRequired()
                .HasDefaultValue(0);

            builder
                .HasMany(p => p.Supplies)
                .WithOne(s => s.Product)
                .HasForeignKey(s => s.ProductId);

            builder
                .HasMany(p => p.ProductsOrders)
                .WithOne(po => po.Product)
                .HasForeignKey(po => po.ProductId);
        }
    }
}
