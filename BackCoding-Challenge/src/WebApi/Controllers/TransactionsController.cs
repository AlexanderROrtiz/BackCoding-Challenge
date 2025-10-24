using BackCoding.Challenge.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BackCoding.Challenge.WebApi.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _repository;
        private readonly Serilog.ILogger _logger;

        public TransactionsController(ITransactionRepository repository)
        {
            _repository = repository;
            _logger = Log.ForContext<TransactionsController>();
        }

        /// <summary>
        /// Obtener todas las transacciones registradas.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                _logger.Information("Obteniendo lista de transacciones...");
                var transactions = await _repository.GetAllAsync();

                if (!transactions.Any())
                {
                    _logger.Warning("No se encontraron transacciones registradas.");
                    return NotFound(new { message = "No se encontraron transacciones." });
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener las transacciones.");
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}
