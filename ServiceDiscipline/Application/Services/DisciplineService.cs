using ServiceDiscipline.Application.Common;
using ServiceDiscipline.Application.Interfaces;
using ServiceDiscipline.Domain.Entities;
using ServiceDiscipline.Domain.Ports;
using ServiceDiscipline.Domain.Rules;
using System;
using System.Collections.Generic;
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
        public async Task<Result<Discipline>> CreateNewDiscipline(Discipline newDiscipline)
        {
            var validationResult = DisciplineValidationRules.Validate(newDiscipline);

            if (validationResult.IsFailure)
            {
                return Result<Discipline>.Failure(validationResult.Error);
            }

            var createdDiscipline = await _disciplineRepository.CreateAsync(newDiscipline);

            return Result<Discipline>.Success(createdDiscipline);
        }

        public async Task<Result<Discipline>> UpdateDiscipline(Discipline disciplineToUpdate)
        {
            var validationResult = DisciplineValidationRules.Validate(disciplineToUpdate);
            if (validationResult.IsFailure)
            {
                return Result<Discipline>.Failure(validationResult.Error);
            }

            var existingDiscipline = await _disciplineRepository.GetByIdAsync(disciplineToUpdate.Id);
            if (existingDiscipline == null)
            {
                return Result<Discipline>.Failure($"No se encontró la disciplina con ID {disciplineToUpdate.Id} para actualizar.");
            }

            var updatedDiscipline = await _disciplineRepository.UpdateAsync(disciplineToUpdate);

            return Result<Discipline>.Success(updatedDiscipline);
        }

        public async Task<Result<Discipline>> GetDisciplineById(int id)
        {
            var discipline = await _disciplineRepository.GetByIdAsync(id);
            if (discipline == null)
            {
                return Result<Discipline>.Failure($"No se encontró la disciplina con ID {id}.");
            }
            return Result<Discipline>.Success(discipline);
        }

        public async Task<Result<bool>> DeleteDiscipline(int id)
        {
            var success = await _disciplineRepository.DeleteByIdAsync(id);
            if (!success)
            {
                return Result<bool>.Failure($"No se pudo eliminar la disciplina con ID {id}.");
            }
            return Result<bool>.Success(true);
        }
        public Task<IEnumerable<Discipline>> GetAllDisciplines() => _disciplineRepository.GetAllAsync();

        //public Task<Discipline> GetDisciplineById(int id) => _disciplineRepository.GetByIdAsync(id);
        //public Task<Discipline> CreateNewDiscipline(Discipline newDiscipline) => _disciplineRepository.CreateAsync(newDiscipline);
        //public Task<bool> DeleteDiscipline(int id) => _disciplineRepository.DeleteByIdAsync(id);
        //public async Task<bool> UpdateDisciplineData(Discipline disciplineToUpdate)
        //{
        //    var updatedDiscipline = await _disciplineRepository.UpdateAsync(disciplineToUpdate);
        //    return updatedDiscipline != null;
        //}
    }
}
