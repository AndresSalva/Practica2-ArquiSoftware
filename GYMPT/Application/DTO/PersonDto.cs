namespace GYMPT.Application.DTO
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstLastname { get; set; }
        public string SecondLastname { get; set; }
        public string Ci { get; set; }
        public DateTime? DateBirth { get; set; }
        public string Role { get; set; }  // "Client", "Instructor", "Admin"
    }
}
