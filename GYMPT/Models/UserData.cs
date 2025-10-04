
namespace GYMPT.Models
{


    public class UserData
    {
        public long Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastModification { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Name { get; set; }

        public string? FirstLastname { get; set; }

        public string? SecondLastname { get; set; }

        public DateTime? DateBirth { get; set; }

        public string? CI { get; set; }

        public string? Role { get; set; }
    }
}