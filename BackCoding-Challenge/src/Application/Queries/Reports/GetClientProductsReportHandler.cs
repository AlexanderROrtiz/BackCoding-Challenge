using BackCoding.Challenge.Application.DTOs.Clients;
using BackCoding.Challenge.Application.Interfaces;
using MediatR;
using Serilog;

namespace BackCoding.Challenge.Application.Queries.Reports
{
    public class GetClientProductsReportHandler : IRequestHandler<GetClientProductsReportQuery, IEnumerable<ClientProductReportDto>>
    {
        private readonly IClientProductsReportRepository _repository;
        private readonly ILogger _logger;

        public GetClientProductsReportHandler(IClientProductsReportRepository repository)
        {
            _repository = repository;
            _logger = Log.ForContext<GetClientProductsReportHandler>();
        }

        public async Task<IEnumerable<ClientProductReportDto>> Handle(GetClientProductsReportQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Ejecutando consulta de reporte de clientes y productos disponibles...");
            var report = await _repository.GetClientProductsReportAsync();
            return report;
        }
    }
}
