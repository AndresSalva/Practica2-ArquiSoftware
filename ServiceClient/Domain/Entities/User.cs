// Ruta: ServiceClient/Domain/Entities/User.cs
// ¡ESTE ARCHIVO ES CORRECTO, NO SE NECESITAN CAMBIOS!
namespace ServiceClient.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastModification { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Name { get; set; }
        public string? FirstLastname { get; set; }
        public string? SecondLastname { get; set; }
        public DateTime? DateBirth { get; set; }
        public string? Ci { get; set; }
        public string? Role { get; set; }
    }
}