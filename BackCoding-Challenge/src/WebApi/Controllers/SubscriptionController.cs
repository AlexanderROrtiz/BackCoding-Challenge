using BackCoding.Challenge.Application.Commands.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BackCoding.Challenge.WebApi.Controllers
{
    [Authorize(Roles = "Cliente,Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public SubscriptionController(IMediator mediator)
        {
            _mediator = mediator;
            _logger = Log.ForContext<SubscriptionController>();
        }

        /// <summary>
        /// Suscribir un cliente a un fondo (Apertura)
        /// </summary>
        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeProductCommand command)
        {
            _logger.Information("Iniciando suscripción del cliente {@Command}", command);

            var result = await _mediator.Send(command);

            _logger.Information("Suscripción completada. Cliente {ClientId}, Producto {ProductId}", result.ClientId, result.ProductId);
            return Ok(result);
        }

        /// <summary>
        /// Cancelar una suscripción (Cancelación)
        /// </summary>
        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelSubscriptionCommand command)
        {
            _logger.Information("Iniciando cancelación {@Command}", command);

            var result = await _mediator.Send(command);

            _logger.Information("Cancelación completada. Cliente {ClientId}, Producto {ProductId}", result.ClientId, result.ProductId);
            return Ok(result);
        }
    }
}
