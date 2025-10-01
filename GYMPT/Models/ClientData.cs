
namespace GYMPT.Models
{

    public class ClientData
    {
        public long IdUser { get; set; }

        public string? FitnessLevel { get; set; }

        public float? InitialWeightKg { get; set; }

        public float? CurrentWeightKg { get; set; }

        public string? EmergencyContactPhone { get; set; }
    }
}