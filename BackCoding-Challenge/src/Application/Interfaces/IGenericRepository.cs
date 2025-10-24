using System.Linq.Expressions;

namespace BackCoding.Challenge.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetQuery();
        Task<T> GetByIdAsync(params object[] keyValues);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
        Task DeleteAsync(T entity);
    }
}
