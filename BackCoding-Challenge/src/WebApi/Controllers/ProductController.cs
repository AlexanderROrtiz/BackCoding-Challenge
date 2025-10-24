using BackCoding.Challenge.Application.Commands.Products;
using BackCoding.Challenge.Application.DTOs.Products;
using BackCoding.Challenge.Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BackCoding.Challenge.WebApi.Controllers
{
    [Authorize(Roles = "Cliente,Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
            _logger = Log.ForContext<ProductController>();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            _logger.Information("Creando producto: {@Request}", request);

            var command = new CreateProductCommand(request);
            var result = await _mediator.Send(command);

            _logger.Information("Producto creado con ID: {Id}", result.Id);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate([FromBody] List<CreateProductRequest> requests)
        {
            if (requests == null || !requests.Any())
                return BadRequest("Debe enviar al menos un producto.");

            var created = new List<object>();
            foreach (var request in requests)
            {
                var result = await _mediator.Send(new CreateProductCommand(request));
                created.Add(result);
            }

            _logger.Information("Se crearon {Count} productos correctamente", created.Count);
            return Ok(new { Created = created.Count, Data = created });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
