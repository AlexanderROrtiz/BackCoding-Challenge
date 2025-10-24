using BackCoding.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackCoding.Challenge.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("usuario");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnName("id_usuario");
            builder.Property(u => u.Username).HasColumnName("usuario").IsRequired().HasMaxLength(100);
            builder.Property(u => u.PasswordHash).HasColumnName("clave_hash").IsRequired();
            builder.Property(u => u.Role).HasColumnName("rol").IsRequired().HasMaxLength(50);
        }
    }
}
