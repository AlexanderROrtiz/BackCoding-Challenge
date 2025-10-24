using BackCoding.Challenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BackCoding.Challenge.Infrastructure.Persistence.Context
{
    public class BackCodingDbContext : DbContext
    {
        public BackCodingDbContext(DbContextOptions<BackCodingDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations from this assembly
            modelBuilder.HasPostgresExtension("pgcrypto");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
