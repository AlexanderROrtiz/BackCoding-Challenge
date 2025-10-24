using BackCoding.Challenge.Application.DTOs.Transactions;
using BackCoding.Challenge.Application.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Serilog;

namespace BackCoding.Challenge.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public TransactionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
            _logger = Log.ForContext<TransactionRepository>();
        }

        public async Task<IEnumerable<TransactionDto>> GetAllAsync()
        {
            const string query = @"
                SELECT 
                    t.id, 
                    c.nombre AS cliente,
                    p.nombre AS producto,
                    t.amount,
                    t.date,
                    t.type
                FROM transactions t
                INNER JOIN cliente c ON c.id_cliente = t.client_id
                INNER JOIN producto p ON p.id_producto = t.product_id
                ORDER BY t.date DESC;";

            await using var connection = new NpgsqlConnection(_connectionString);
            var result = await connection.QueryAsync<TransactionDto>(query);

            _logger.Information("Se recuperaron {Count} transacciones.", result.Count());
            return result;
        }
    }
}
