using BackCoding.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackCoding.Challenge.Infrastructure.Persistence.Configurations
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("inscripcion");

            builder.HasKey(s => new { s.ProductId, s.ClientId });

            builder.Property(s => s.ProductId).HasColumnName("id_producto");
            builder.Property(s => s.ClientId).HasColumnName("id_cliente");

            builder.HasOne(s => s.Product)
                   .WithMany(p => p.Subscriptions)
                   .HasForeignKey(s => s.ProductId);

            builder.HasOne(s => s.Client)
                   .WithMany(c => c.Subscriptions)
                   .HasForeignKey(s => s.ClientId);
        }
    }
}
