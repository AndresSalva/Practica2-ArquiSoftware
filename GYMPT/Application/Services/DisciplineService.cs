using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;

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
        public Task CreateNewDiscipline(Discipline newDiscipline) => _disciplineRepository.CreateAsync(newDiscipline);
        public Task UpdateDisciplineData(Discipline disciplineToUpdate) => _disciplineRepository.UpdateAsync(disciplineToUpdate);
        public Task DeleteDiscipline(int id) => _disciplineRepository.DeleteByIdAsync(id);
    }
}