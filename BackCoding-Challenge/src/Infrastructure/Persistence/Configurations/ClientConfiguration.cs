using BackCoding.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackCoding.Challenge.Infrastructure.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("cliente");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("id_cliente").ValueGeneratedOnAdd();
            builder.Property(c => c.FirstName).HasColumnName("nombre").IsRequired().HasMaxLength(100);
            builder.Property(c => c.LastName).HasColumnName("apellidos").IsRequired().HasMaxLength(100);
            builder.Property(c => c.City).HasColumnName("ciudad").HasMaxLength(100);
            builder.Property(c => c.Balance).HasColumnName("balance").HasColumnType("numeric(18,2)").HasDefaultValue(500000M).IsRequired();
            builder.Property(c => c.PhoneNumber).HasColumnName("telefono").HasColumnType("text").IsRequired();
        }
    }
}
