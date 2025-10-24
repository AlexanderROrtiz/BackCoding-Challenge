
namespace BackCoding.Challenge.Domain.Entities
{
    public class Availability
    {
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
