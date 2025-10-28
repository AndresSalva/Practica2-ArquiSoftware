using System.ComponentModel.DataAnnotations;
namespace ServiceDiscipline.Domain.Entities
{
    public class Discipline
    {
        public short Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModification { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Name { get; set; }
        public long? IdInstructor { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
    }
}