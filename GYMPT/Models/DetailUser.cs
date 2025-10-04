
namespace GYMPT.Models
{
    public class DetailsUser 
    {

        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastModification { get; set; }

        public bool IsActive { get; set; } = true;

        public long IdUser { get; set; }

        public long IdMembership { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? SessionsLeft { get; set; }
    }
}
