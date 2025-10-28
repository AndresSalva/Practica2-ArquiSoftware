using ServiceUser.Domain.Entities;

namespace GYMPT.Domain.Entities
{
    public class Client : User
    {
        public int IdUser { get; set; }
        public string? FitnessLevel { get; set; }
        public decimal? InitialWeightKg { get; set; }
        public decimal? CurrentWeightKg { get; set; }
        public string? EmergencyContactPhone { get; set; }
    }
}