using BackCoding.Challenge.Application.Interfaces;
using Serilog;

namespace BackCoding.Challenge.Infrastructure.Services
{
    public class SmsNotificationService : INotificationService
    {
        public async Task SendNotificationAsync(string recipient, string subject, string message)
        {
            // Simulación de envío SMS
            await Task.Delay(200);
            Log.Information($"📱 SMS enviado a {recipient}: {subject} - {message}");
        }
    }
}
