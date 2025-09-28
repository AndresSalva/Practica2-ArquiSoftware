using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations.Schema;


//[Table ("Discipline")]
namespace GYMPT.Model
{
    public class Discipline
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastModification { get; set; }

        public bool? IsActive { get; set; }

        [MaxLength(150)]
        public string? Name { get; set; }

        public string IdInstructor { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

    }
}