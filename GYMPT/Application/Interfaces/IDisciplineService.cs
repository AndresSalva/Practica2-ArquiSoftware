using GYMPT.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Application.Interfaces
{
    /// <summary>
    /// Define el contrato estandarizado para los servicios que gestionan las disciplinas.
    /// </summary>
    public interface IDisciplineService
    {
        /// <summary>
        /// Obtiene una disciplina por su identificador único.
        /// </summary>
        /// <param name="id">El ID de la disciplina a buscar.</param>
        /// <returns>La disciplina si se encuentra; de lo contrario, null.</returns>
        Task<Discipline?> GetByIdAsync(int id);

        /// <summary>
        /// Obtiene una lista de todas las disciplinas.
        /// </summary>
        /// <returns>Una colección de entidades Discipline.</returns>
        Task<IEnumerable<Discipline>> GetAllAsync();

        /// <summary>
        /// Crea una nueva disciplina.
        /// </summary>
        /// <param name="discipline">La entidad Discipline a crear.</param>
        /// <returns>La entidad Discipline creada, con su ID asignado.</returns>
        Task<Discipline> CreateAsync(Discipline discipline);

        Task<Discipline?> UpdateAsync(Discipline discipline);

        Task<bool> DeleteByIdAsync(int id);
    }
}