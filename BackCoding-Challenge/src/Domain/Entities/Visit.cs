
namespace BackCoding.Challenge.Domain.Entities
{
    public class Visit
    {
        public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public DateTime VisitDate { get; set; } // fecha_visita
    }
}
