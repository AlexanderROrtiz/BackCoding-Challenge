using BackCoding.Challenge.Application.Interfaces;
using BackCoding.Challenge.Application.Queries.Reports;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BackCoding.Challenge.WebApi.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Serilog.ILogger _logger;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
            _logger = Log.ForContext<ReportsController>();
        }

        [HttpGet("client-products")]
        public async Task<IActionResult> GetClientProductsReport()
        {
            _logger.Information("Solicitando reporte de clientes con productos y sucursales disponibles");
            var query = new GetClientProductsReportQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
