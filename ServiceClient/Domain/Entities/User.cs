// Ruta: ServiceClient/Domain/Entities/User.cs

using System;

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
        public string? Ci { get; set; } // Cédula de Identidad
        public string? Role { get; set; }
    }
}