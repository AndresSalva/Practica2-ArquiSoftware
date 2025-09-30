using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;

namespace GYMPT.Models
{
    // Esta clase es un DTO. Su única misión es mapear 1 a 1 con la tabla "Instructor".
    [Table("Instructor")]
    public class InstructorData : BaseModel
    {
        [PrimaryKey("id_user")]
        public long IdUser { get; set; }

        [Column("hire_date")]
        public DateTime? HireDate { get; set; }

        [Column("monthly_salary")]
        public float? MonthlySalary { get; set; }

        [Column("specialization")]
        public string? Specialization { get; set; }
    }
}