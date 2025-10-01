using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace GYMPT.Models
{
    [Table("Details_user")]
    public class DetailsUser : BaseModel
    {
        [PrimaryKey("id")]
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("last_modification")]
        public DateTime? LastModification { get; set; }

        [Column("isActive")]
        public bool? IsActive { get; set; }

        [Column("id_user")]
        public long IdUser { get; set; }

        [Column("id_membership")]
        public long IdMembership { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("sessions_left")]
        public int? SessionsLeft { get; set; }
    }
}
