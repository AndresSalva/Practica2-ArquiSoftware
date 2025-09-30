using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using System;

namespace GYMPT.Models
{

    [Table("User")]
    public class UserData : BaseModel
    {
        [PrimaryKey]
        public long Id { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("last_modification")]
        public DateTime? LastModification { get; set; }

        [Column("isActive")]
        public bool? IsActive { get; set; }

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

        [Column("role")]
        public string? Role { get; set; }
    }
}