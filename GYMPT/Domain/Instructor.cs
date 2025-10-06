using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Models;
using System;



namespace GYMPT.Domain
{
    public class Instructor : User
    {
        [Column("hire_date")]
        public DateTime? HireDate { get; set; }

        [Column("monthly_salary")]
        public float? MonthlySalary { get; set; }

        [Column("specialization")]
        public string? Specialization { get; set; }
    }
}
