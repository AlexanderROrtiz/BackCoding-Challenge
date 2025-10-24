using BackCoding.Challenge.Application.DTOs.Clients;
using BackCoding.Challenge.Application.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace BackCoding.Challenge.Infrastructure.Reports
{
    public class ClientProductsReportRepository : IClientProductsReportRepository
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public ClientProductsReportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("DefaultConnection");
            _logger = Log.ForContext<ClientProductsReportRepository>();
        }

        public async Task<IEnumerable<ClientProductReportDto>> GetClientProductsReportAsync()
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            var result = await connection.QueryAsync<ClientProductReportDto>(
                "SELECT * FROM fn_clientes_productos_sucursales();");

            _logger.Information("Reporte ejecutado correctamente: {Count} registros encontrados", result.Count());

            return result;
        }
    }

}
