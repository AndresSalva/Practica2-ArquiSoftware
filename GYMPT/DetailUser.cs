using System;

namespace GYMPT
{
    internal class DetailsUser
    {
        #region Atributos
        private long id;
        private DateTime createdAt;
        private DateTime lastModification;
        private bool isActive;
        private long idUser;
        private long idMembership;
        private DateTime startDate;
        private DateTime endDate;
        private short sessionsLeft;
        #endregion

        #region Propiedades
        public long Id { get => id; set => id = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime LastModification { get => lastModification; set => lastModification = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public long IdUser { get => idUser; set => idUser = value; }
        public long IdMembership { get => idMembership; set => idMembership = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public short SessionsLeft { get => sessionsLeft; set => sessionsLeft = value; }

        // Navegación: el usuario al que pertenece este detalle
        public User User { get; set; }
        #endregion

        #region Constructores
        public DetailsUser()
        {
        }

        public DetailsUser(long id, DateTime createdAt, DateTime lastModification, bool isActive, long idUser, long idMembership, DateTime startDate, DateTime endDate, short sessionsLeft)
        {
            Id = id;
            CreatedAt = createdAt;
            LastModification = lastModification;
            IsActive = isActive;
            IdUser = idUser;
            IdMembership = idMembership;
            StartDate = startDate;
            EndDate = endDate;
            SessionsLeft = sessionsLeft;
        }
        #endregion

        #region Métodos
        public virtual string ObtenerDatosCompletos()
        {
            return $"DetalleID: {Id} | UsuarioID: {IdUser} | MembresíaID: {IdMembership} | Inicio: {StartDate} | Fin: {EndDate} | Sesiones Restantes: {SessionsLeft}";
        }
        #endregion
    }
}
