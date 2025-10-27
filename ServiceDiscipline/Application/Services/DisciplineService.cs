using System;
using System.Collections.Generic;
using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Domain.Entities;
using ServiceDiscipline.Domain.Ports;
using System.Threading.Tasks;

namespace ServiceDiscipline.Application.Services
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
