using BackCoding.Challenge.Application.DTOs.Clients;
using MediatR;

namespace BackCoding.Challenge.Application.Queries.Reports
{
    public record GetClientProductsReportQuery : IRequest<IEnumerable<ClientProductReportDto>>;
}
