using BackCoding.Challenge.Application.Interfaces;
using Serilog;

namespace BackCoding.Challenge.Infrastructure.Services
{
    public class EmailNotificationService : INotificationService
    {
        public async Task SendNotificationAsync(string recipient, string subject, string message)
        {
            // Simulación de envío de correo
            await Task.Delay(200);
            Log.Information($"📧 Email enviado a {recipient}: {subject} - {message}");
        }
    }
}
