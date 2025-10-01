using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace GYMPT.Models
{
    [Table("User")]
    public class User : BaseModel
    {
        [PrimaryKey] // Clave primaria
        public long Id { get; set; }   

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }   // timestamptz -> DateTime

        [Column("last_modification")]
        public DateTime? LastModification { get; set; }   // timestamp -> nullable

        [Column("isActive")]
        public bool? IsActive { get; set; }   // int4 -> bool? (nullable)

        [Column("name")]
        public string? Name { get; set; }

        [Column("first_lastname")]
        public string? FirstLastname { get; set; }

        [Column("second_lastname")]
        public string? SecondLastname { get; set; }

        [Column("date_birth")]
        public DateTime? DateBirth { get; set; }   

        [Column("CI")]
        public string? CI { get; set; }   
    }
}
