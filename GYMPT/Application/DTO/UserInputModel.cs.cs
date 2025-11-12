namespace GYMPT.Application.DTO
{
    public class UserInputModel
    {
        public string Role { get; set; } = "";
        public string Name { get; set; } = "";
        public string FirstLastname { get; set; } = "";
        public string? SecondLastname { get; set; } = "";
        public string Ci { get; set; } = "";
        public DateTime DateBirth { get; set; }

        // Cliente
        public string? FitnessLevel { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public decimal? InitialWeightKg { get; set; }
        public decimal? CurrentWeightKg { get; set; }

        // Instructor
        public string? Specialization { get; set; }
        public DateTime HireDate { get; set; }
        public decimal? MonthlySalary { get; set; }
        public string? Email { get; set; }
    }

}
