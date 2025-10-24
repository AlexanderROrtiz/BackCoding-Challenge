using BackCoding.Challenge.Application.DTOs.Transactions;

namespace BackCoding.Challenge.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<TransactionDto>> GetAllAsync();
    }
}
