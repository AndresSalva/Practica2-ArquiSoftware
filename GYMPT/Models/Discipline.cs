
namespace GYMPT.Models
{
    public class Discipline
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastModification { get; set; }

        public bool? IsActive { get; set; }

        public string? Name { get; set; }

        public long? IdInstructor { get; set; }

        public TimeSpan? StartTime { get; set; }

        public TimeSpan? EndTime { get; set; }
    }
}