using GYMPT.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Application.Interfaces
{
    public interface IDisciplineService
    {
        Task<Discipline> GetDisciplineById(int id);
        Task<IEnumerable<Discipline>> GetAllDisciplines();
        Task<Discipline> CreateNewDiscipline(Discipline newDiscipline);
        Task<bool> UpdateDisciplineData(Discipline disciplineToUpdate);
        Task<bool> DeleteDiscipline(int id);
    }
}