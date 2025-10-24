using BackCoding.Challenge.Application.DTOs.Clients;

namespace BackCoding.Challenge.Application.Interfaces
{
    public interface IClientProductsReportRepository
    {
        Task<IEnumerable<ClientProductReportDto>> GetClientProductsReportAsync();
    }
}
