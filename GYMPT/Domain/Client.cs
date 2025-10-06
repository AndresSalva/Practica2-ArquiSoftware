using Supabase.Postgrest.Attributes; 
using Supabase.Postgrest.Models;
using System;

namespace GYMPT.Domain
{
    public class Client : User
    {
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
