namespace GYMPT.Domain
{
    public class Client: User
    {
        public string? FitnessLevel { get; set; }

        public float? InitialWeightKg { get; set; }

        public float? CurrentWeightKg { get; set; }

        public string? EmergencyContactPhone { get; set; }
    }
}
