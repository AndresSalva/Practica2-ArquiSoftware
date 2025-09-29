using System;
using System.Collections.Generic;

namespace GYMPT
{
    internal class User
    {
        #region Atributos
        private long id;
        private string name;
        private bool isActive;
        private DateTime createdAt;
        private DateTime lastModification;
        #endregion

        #region Propiedades
        public long Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime LastModification { get => lastModification; set => lastModification = value; }

        // Relación: un usuario puede tener varios detalles/membresías
        public List<DetailsUser> Detalles { get; set; } = new List<DetailsUser>();
        #endregion

        #region Constructores
        public User()
        {
        }

        public User(long id, string name, bool isActive, DateTime createdAt, DateTime lastModification)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
            CreatedAt = createdAt;
            LastModification = lastModification;
        }
        #endregion

        #region Métodos
        public virtual string ObtenerDatosCompletos()
        {
            return $"ID: {Id} | Nombre: {Name} | Activo: {IsActive} | Creado: {CreatedAt} | Última Modificación: {LastModification}";
        }
        #endregion
    }
}
