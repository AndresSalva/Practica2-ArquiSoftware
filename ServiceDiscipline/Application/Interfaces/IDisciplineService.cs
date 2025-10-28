using ServiceDiscipline.Application.Common;
using ServiceDiscipline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
