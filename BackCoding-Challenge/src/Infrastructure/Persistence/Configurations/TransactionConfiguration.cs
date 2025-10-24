using BackCoding.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackCoding.Challenge.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()"); // requires pgcrypto or extension
            builder.Property(t => t.ClientId).HasColumnName("client_id");
            builder.Property(t => t.ProductId).HasColumnName("product_id");
            builder.Property(t => t.Amount).HasColumnName("amount").HasColumnType("numeric(18,2)");
            builder.Property(t => t.Date).HasColumnName("date");
            builder.Property(t => t.Type).HasColumnName("type").HasMaxLength(50);

            builder.HasOne(t => t.Client).WithMany().HasForeignKey(t => t.ClientId);
            builder.HasOne(t => t.Product).WithMany().HasForeignKey(t => t.ProductId);
        }
    }
}
