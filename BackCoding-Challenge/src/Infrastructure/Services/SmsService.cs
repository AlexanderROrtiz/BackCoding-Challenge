using BackCoding.Challenge.Application.Interfaces;

namespace BackCoding.Challenge.Infrastructure.Services
{
    public class SmsService : ISmsService
    {
        public Task SendSmsAsync(string phoneNumber, string message)
        {
            Console.WriteLine($"📱 SMS enviado a {phoneNumber}: {message}");
            return Task.CompletedTask;
        }
    }
}
