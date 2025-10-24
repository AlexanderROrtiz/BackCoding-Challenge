
namespace BackCoding.Challenge.Application.DTOs.Subscriptions
{
    public class SubscriptionRequest
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public decimal Amount { get; set; }
    }

    public class SubscriptionResponseDto
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public Guid TransactionId { get; set; }
        public decimal NewBalance { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
