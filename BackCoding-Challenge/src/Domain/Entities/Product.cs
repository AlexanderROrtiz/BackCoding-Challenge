
namespace BackCoding.Challenge.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; } // id_producto
        public string Name { get; set; }
        public string ProductType { get; set; } // tipo_producto
        public decimal MinAmount { get; set; } // min_amount

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
    }
}
