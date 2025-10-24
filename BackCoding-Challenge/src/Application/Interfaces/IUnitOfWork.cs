
namespace BackCoding.Challenge.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        IGenericRepository<T> Repository<T>() where T : class;
    }
}
