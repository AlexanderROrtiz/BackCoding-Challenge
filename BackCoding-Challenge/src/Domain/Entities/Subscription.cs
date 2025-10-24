
namespace BackCoding.Challenge.Domain.Entities
{
    public class Subscription
    {
        // composite key (ClientId, ProductId) mapped via Fluent API
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
