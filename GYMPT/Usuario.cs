using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYMPT.Model
{
    //[Table("Usuario")] // Si quieres mapear con un nombre exacto en BD
    public class Usuario
    {
        [Key] // Clave primaria
        public long Id { get; set; }   

        [Required]
        public DateTime CreatedAt { get; set; }   // timestamptz -> DateTime

        public DateTime? LastModification { get; set; }   // timestamp -> nullable

        public bool? IsActive { get; set; }   // int4 -> bool? (nullable)

        [MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(150)]
        public string? FirstLastname { get; set; }

        [MaxLength(150)]
        public string? SecondLastname { get; set; }

        public DateTime? DateBirth { get; set; }   

        [MaxLength(50)]
        public string? CI { get; set; }   
    }
}
