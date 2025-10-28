// Ruta: ServiceClient/Domain/Entities/Client.cs

namespace ServiceClient.Domain.Entities
{
    public class Client : User
    {
        public string? FitnessLevel { get; set; }
        public decimal? InitialWeightKg { get; set; }
        public decimal? CurrentWeightKg { get; set; }
        public string? EmergencyContactPhone { get; set; }
    }
}