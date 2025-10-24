using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BackCoding.Challenge.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BackCodingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(BackCodingDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> GetQuery() => _dbSet.AsQueryable();
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(params object[] keyValues)
            => await _dbSet.FindAsync(keyValues);

        public void Remove(T entity) => _dbSet.Remove(entity);

        public void Update(T entity) => _dbSet.Update(entity);
    }
}
