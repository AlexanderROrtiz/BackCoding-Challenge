using BackCoding.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackCoding.Challenge.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("producto");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id_producto");
            builder.Property(p => p.Name).HasColumnName("nombre").IsRequired().HasMaxLength(200);
            builder.Property(p => p.ProductType).HasColumnName("tipo_producto").HasMaxLength(100);
            builder.Property(p => p.MinAmount).HasColumnName("min_amount").HasColumnType("numeric(18,2)");
        }
    }
}
