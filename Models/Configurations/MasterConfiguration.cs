using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SUPIR_RAZOR.Models.Entities;

namespace SUPIR_RAZOR.Models.Configurations
{
    public class MasterConfiguration : IEntityTypeConfiguration<MasterEntity>
    {
        public void Configure(EntityTypeBuilder<MasterEntity> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(m => m.PhoneNumber)
                .IsRequired()
                .HasMaxLength(12);

            builder
                .HasMany(m => m.Supplies)
                .WithOne(s => s.Master)
                .HasForeignKey(s => s.MasterId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
