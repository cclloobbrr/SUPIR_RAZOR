using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Models.Configurations
{
    public class SupplyConfiguration : IEntityTypeConfiguration<SupplyEntity>
    {
        public void Configure(EntityTypeBuilder<SupplyEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(s => s.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(s => s.Date)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(s => s.MasterId)
                .IsRequired();

            builder.Property(s => s.ProductId)
                .IsRequired();

            builder
                .HasOne(s => s.Master)
                .WithMany(m => m.Supplies)
                .HasForeignKey(s => s.MasterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(s => s.Product)
                .WithMany(p => p.Supplies)
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
