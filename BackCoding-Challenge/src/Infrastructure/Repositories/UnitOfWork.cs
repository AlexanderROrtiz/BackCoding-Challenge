using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Infrastructure.Persistence.Context;
using System.Collections.Concurrent;

namespace BackCoding.Challenge.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BackCodingDbContext _context;
        private readonly ConcurrentDictionary<string, object> _repositories;

        public UnitOfWork(BackCodingDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (_repositories.TryGetValue(type, out var repo))
                return (IGenericRepository<T>)repo;

            var instance = new GenericRepository<T>(_context);
            _repositories.TryAdd(type, instance);
            return instance;
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
