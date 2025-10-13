using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Application.Services
{
    public class DisciplineService : IDisciplineService
    {
        private readonly IDisciplineRepository _disciplineRepository;

        public DisciplineService(IDisciplineRepository disciplineRepository)
        {
            _disciplineRepository = disciplineRepository;
        }

        public Task<Discipline> GetDisciplineById(int id) => _disciplineRepository.GetByIdAsync(id);
        public Task<IEnumerable<Discipline>> GetAllDisciplines() => _disciplineRepository.GetAllAsync();
        public Task<Discipline> CreateNewDiscipline(Discipline newDiscipline) => _disciplineRepository.CreateAsync(newDiscipline);
        public Task<bool> DeleteDiscipline(int id) => _disciplineRepository.DeleteByIdAsync(id);
        public async Task<bool> UpdateDisciplineData(Discipline disciplineToUpdate)
        {
            var updatedDiscipline = await _disciplineRepository.UpdateAsync(disciplineToUpdate);
            return updatedDiscipline != null;
        }
    }
}