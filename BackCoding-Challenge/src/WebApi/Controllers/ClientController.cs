using BackCoding.Challenge.Application.Commands.Clients;
using BackCoding.Challenge.Application.Queries.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BackCoding.Challenge.WebApi.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
            _logger = Log.ForContext<ClientController>();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateClientCommand command)
        {
            _logger.Information("Iniciando creación de cliente {@Command}", command);

            var result = await _mediator.Send(command);

            _logger.Information("Cliente creado exitosamente con Id {Id}", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate([FromBody] List<CreateClientCommand> commands)
        {
            if (commands == null || !commands.Any())
                return BadRequest("Debe enviar al menos un cliente.");

            var created = new List<object>();
            foreach (var command in commands)
            {
                var result = await _mediator.Send(command);
                created.Add(result);
            }

            _logger.Information("Se crearon {Count} clientes correctamente", created.Count);
            return Ok(new { Created = created.Count, Data = created });
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.Information("Consultando cliente con Id {Id}", id);

            var query = new GetClientByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result == null)
            {
                _logger.Warning("Cliente con Id {Id} no encontrado", id);
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.Information("Consultando todos los clientes");

            var query = new GetAllClientsQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
