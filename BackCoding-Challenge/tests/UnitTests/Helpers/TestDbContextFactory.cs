using BackCoding.Challenge.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BackCoding.Challenge.Tests.Unit.Helpers
{
    public static class TestDbContextFactory
    {
        public static BackCodingDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<BackCodingDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new BackCodingDbContext(options);
        }
    }
}
