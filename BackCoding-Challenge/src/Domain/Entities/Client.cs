
namespace BackCoding.Challenge.Domain.Entities
{
    public class Client
    {
        public int Id { get; set; } // map to id_cliente
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public decimal Balance { get; set; } = 500000M;
        public string PhoneNumber { get; set; }

        // navigations
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }
}
