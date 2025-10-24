using BackCoding.Challenge.Domain.Entities;
using BackCoding.Challenge.Infrastructure.Persistence.Context;

namespace BackCoding.Challenge.Infrastructure.Authentication
{
    public class AuthService
    {
        private readonly BackCodingDbContext _context;
        private readonly JwtTokenGenerator _jwtGenerator;

        public AuthService(BackCodingDbContext context, JwtTokenGenerator jwtGenerator)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<User> RegisterAsync(string username, string password, string role = "Cliente")
        {
            var existing = _context.Users.FirstOrDefault(u => u.Username == username);
            if (existing != null)
                throw new InvalidOperationException("El usuario ya existe.");

            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User { Username = username, PasswordHash = hash, Role = role };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Credenciales inválidas.");

            return _jwtGenerator.GenerateToken(user);
        }
    }
}
