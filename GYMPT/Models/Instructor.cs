namespace GYMPT.Models
{
    public class Instructor : User
    {
        public int IdUser { get; set; }
        public DateTime? HireDate { get; set; }
        public decimal? MonthlySalary { get; set; }
        public string? Specialization { get; set; }
    }
}
