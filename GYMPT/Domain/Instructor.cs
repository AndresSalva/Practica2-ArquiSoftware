using System;

namespace GYMPT.Domain
{
    public class Instructor : User
    {

        public DateTime? HireDate { get; set; }

        public float? MonthlySalary { get; set; }

        public string? Specialization { get; set; }
    }
}