using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace GYMPT.Models
{

    [Table("Membership")]
    public class Membership : BaseModel 
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

        [Column("price")]
        public float? Price { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("monthly_sessions")]
        public short? MonthlySessions { get; set; }
    }
}