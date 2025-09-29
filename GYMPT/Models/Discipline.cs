using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace GYMPT.Models
{
    [Table("Discipline")]
    public class Discipline : BaseModel
    {
        [PrimaryKey("id")]
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("last_modification")]
        public DateTime? LastModification { get; set; }

        [Column("isActive")]
        public bool? IsActive { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("id_instructor")]
        public long? IdInstructor { get; set; }

        [Column("start_time")]
        public TimeOnly? StartTime { get; set; }

        [Column("end_time")]
        public TimeOnly? EndTime { get; set; }
    }
}