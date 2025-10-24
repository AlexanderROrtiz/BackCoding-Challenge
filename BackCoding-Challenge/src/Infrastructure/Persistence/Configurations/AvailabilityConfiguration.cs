
using BackCoding.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackCoding.Challenge.Infrastructure.Persistence.Configurations
{
    public class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
    {
        public void Configure(EntityTypeBuilder<Availability> builder)
        {
            builder.ToTable("disponibilidad");

            builder.HasKey(a => new { a.BranchId, a.ProductId });

            builder.Property(a => a.BranchId).HasColumnName("id_sucursal");
            builder.Property(a => a.ProductId).HasColumnName("id_producto");

            builder.HasOne(a => a.Branch)
                   .WithMany(b => b.Availabilities)
                   .HasForeignKey(a => a.BranchId);

            builder.HasOne(a => a.Product)
                   .WithMany(p => p.Availabilities)
                   .HasForeignKey(a => a.ProductId);
        }
    }
}
