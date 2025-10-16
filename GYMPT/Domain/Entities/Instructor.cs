namespace GYMPT.Domain.Entities
{
    public class Instructor : User
    {
        public int IdUser { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? MonthlySalary { get; set; }
        public string? Specialization { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}