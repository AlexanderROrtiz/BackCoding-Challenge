using BackCoding.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackCoding.Challenge.Infrastructure.Persistence.Configurations
{
    public class VisitConfiguration : IEntityTypeConfiguration<Visit>
    {
        public void Configure(EntityTypeBuilder<Visit> builder)
        {
            builder.ToTable("visitan");

            builder.HasKey(v => new { v.BranchId, v.ClientId, v.VisitDate }); // composite including date

            builder.Property(v => v.BranchId).HasColumnName("id_sucursal");
            builder.Property(v => v.ClientId).HasColumnName("id_cliente");
            builder.Property(v => v.VisitDate).HasColumnName("fecha_visita");

            builder.HasOne(v => v.Branch)
                   .WithMany(b => b.Visits)
                   .HasForeignKey(v => v.BranchId);

            builder.HasOne(v => v.Client)
                   .WithMany(c => c.Visits)
                   .HasForeignKey(v => v.ClientId);
        }
    }
}
