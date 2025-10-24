
namespace BackCoding.Challenge.Domain.Entities
{
    public class Branch
    {
        public int Id { get; set; } // id_sucursal
        public string Name { get; set; }
        public string City { get; set; }

        public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
    }

}
