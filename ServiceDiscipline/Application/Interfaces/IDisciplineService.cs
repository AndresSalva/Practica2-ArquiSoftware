using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceDiscipline.Domain.Entities;
namespace ServiceDiscipline.Application.Interfaces
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
