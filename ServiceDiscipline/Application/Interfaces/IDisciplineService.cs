using ServiceCommon.Application;
using ServiceDiscipline.Domain.Entities;

namespace ServiceDiscipline.Application.Interfaces
{
    public interface IDisciplineService
    {
        Task<IEnumerable<Discipline>> GetAllDisciplines();
        Task<Result<Discipline>> GetDisciplineById(int id);
        Task<Result<Discipline>> CreateNewDiscipline(Discipline newDiscipline);
        Task<Result<Discipline>> UpdateDiscipline(Discipline disciplineToUpdate);
        Task<Result<bool>> DeleteDiscipline(int id);
    }
}
