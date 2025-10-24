using BackCoding.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackCoding.Challenge.Infrastructure.Persistence.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("sucursal");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasColumnName("id_sucursal");
            builder.Property(b => b.Name).HasColumnName("nombre").IsRequired().HasMaxLength(150);
            builder.Property(b => b.City).HasColumnName("ciudad").HasMaxLength(100);
        }
    }
}
