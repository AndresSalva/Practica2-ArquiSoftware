using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
      
namespace GYMPT.Domain
{
    public abstract class User : BaseModel
    {
        [PrimaryKey("id", false)] // <-- Aquí marcas la clave primaria
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("last_modification")]
        public DateTime? LastModification { get; set; }

        [Column("is_active")]
        public bool? IsActive { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("first_lastname")]
        public string? FirstLastname { get; set; }

        [Column("second_lastname")]
        public string? SecondLastname { get; set; }

        [Column("date_birth")]
        public DateTime? DateBirth { get; set; }

        [Column("ci")]
        public string? CI { get; set; }

        [Column("role")]
        public string? Role { get; set; }
    }
}
