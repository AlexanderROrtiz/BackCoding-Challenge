
namespace BackCoding.Challenge.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; } // GUID único
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public string Type { get; set; } // "Apertura" / "Cancelacion"
    }
}
