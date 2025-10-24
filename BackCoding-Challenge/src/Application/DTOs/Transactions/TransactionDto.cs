
namespace BackCoding.Challenge.Application.DTOs.Transactions
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Producto { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = string.Empty;
    }
}
