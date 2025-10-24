using BackCoding.Challenge.Application.Interfaces;

namespace BackCoding.Challenge.Infrastructure.Services
{
    public class NotificationStrategyResolver : INotificationStrategyResolver
    {
        private readonly EmailNotificationService _emailService;
        private readonly SmsNotificationService _smsService;

        public NotificationStrategyResolver(
            EmailNotificationService emailService,
            SmsNotificationService smsService)
        {
            _emailService = emailService;
            _smsService = smsService;
        }

        public INotificationService Resolve(string preference)
        {
            return preference.ToLower() switch
            {
                "email" => _emailService,
                "sms" => _smsService,
                _ => _emailService // default
            };
        }
    }
}
