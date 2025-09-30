using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace GYMPT.Models
{

    [Table("Client")]
    public class ClientData : BaseModel
    {
        [PrimaryKey("id_user")]
        public long IdUser { get; set; }

        [Column("fitness_level")]
        public string? FitnessLevel { get; set; }

        [Column("initial_weight_kg")]
        public float? InitialWeightKg { get; set; }

        [Column("current_weight_kg")]
        public float? CurrentWeightKg { get; set; }

        [Column("emergency_contact_phone")]
        public string? EmergencyContactPhone { get; set; }
    }
}