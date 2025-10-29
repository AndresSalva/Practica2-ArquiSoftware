using ServicePerson.Application.Interfaces;
using ServicePerson.Domain.Entities;
using ServicePerson.Domain.Ports;
using ServicePerson.Application.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicePerson.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<Person>> GetAllPeople()
        {
            return await _personRepository.GetAllAsync();
        }

        public async Task<Result<Person>> GetPersonById(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null)
                return Result<Person>.Failure($"No se encontró la persona con ID {id}.");

            return Result<Person>.Success(person);
        }

        //public async Task<Result<Person>> CreateNewPerson(Person newPerson)
        //{
        //    await _personRepository.AddAsync(newPerson);
        //    return Result<Person>.Success(newPerson);
        //}

        //public async Task<Result<Person>> UpdatePerson(Person personToUpdate)
        //{
        //    var existing = await _personRepository.GetByIdAsync(personToUpdate.Id);
        //    if (existing == null)
        //        return Result<Person>.Failure($"No se encontró la persona con ID {personToUpdate.Id}.");

        //    await _personRepository.UpdateAsync(personToUpdate);
        //    return Result<Person>.Success(personToUpdate);
        //}

        public async Task<Result<bool>> DeletePerson(int id)
        {
            var deleted = await _personRepository.DeleteByIdAsync(id);
            if (!deleted)
                return Result<bool>.Failure($"No se pudo eliminar la persona con ID {id}.");

            return Result<bool>.Success(true);
        }
    }
}
