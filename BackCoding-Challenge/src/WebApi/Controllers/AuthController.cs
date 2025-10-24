using BackCoding.Challenge.Infrastructure.Authentication;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BackCoding.Challenge.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly Serilog.ILogger _logger;

        public AuthController(AuthService authService)
        {
            _authService = authService;
            _logger = Log.ForContext<AuthController>();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.Information("Registrando usuario {Username}", request.Username);
            var user = await _authService.RegisterAsync(request.Username, request.Password, request.Role);
            return Ok(new { user.Id, user.Username, user.Role });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var token = await _authService.LoginAsync(request.Username, request.Password);
            return Ok(new { Token = token });
        }
    }

    public record RegisterRequest(string Username, string Password, string Role = "Cliente");
    public record LoginRequest(string Username, string Password);
}
