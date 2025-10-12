using GYMPT.Domain.Entities;

namespace GYMPT.Application.Interfaces
{
    public interface IDisciplineService
    {
        Task<Discipline> GetDisciplineById(int id);
        Task<IEnumerable<Discipline>> GetAllDisciplines();
        Task CreateNewDiscipline(Discipline newDiscipline);
        Task UpdateDisciplineData(Discipline disciplineToUpdate);
        Task DeleteDiscipline(int id);
    }
}