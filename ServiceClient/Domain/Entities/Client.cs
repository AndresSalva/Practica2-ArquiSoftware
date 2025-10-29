using ServicePerson.Domain.Entities;

namespace ServiceClient.Domain.Entities
{
    public class Client : Person
    {
        public string? FitnessLevel { get; set; }
        public decimal? InitialWeightKg { get; set; }
        public decimal? CurrentWeightKg { get; set; }
        public string? EmergencyContactPhone { get; set; }
    }
}