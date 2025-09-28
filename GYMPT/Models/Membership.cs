using System;
using System.ComponentModel.DataAnnotations;

namespace GYMPT.Models
{
    public class Membership
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastModification { get; set; }

        public bool? IsActive { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        public float? Price { get; set; }

        public string? Description { get; set; }

        public long? MonthlySessions { get; set; }
    }
}
