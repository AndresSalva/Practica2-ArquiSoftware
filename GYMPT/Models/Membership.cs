
namespace GYMPT.Models
{
    public class Membership 
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastModification { get; set; }

        public bool? IsActive { get; set; }

        public string? Name { get; set; }

        public float? Price { get; set; }

        public string? Description { get; set; }

        public short? MonthlySessions { get; set; }
    }
}