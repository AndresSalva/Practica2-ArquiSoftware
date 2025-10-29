using ServicePerson.Application.Common;
using ServicePerson.Domain.Entities;
using System;


namespace ServicePerson.Application.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPeople();
        Task<Result<Person>> GetPersonById(int id);
        //Task<Result<Person>> CreateNewPerson(Person newPerson);
        //Task<Result<Person>> UpdatePerson(Person personToUpdate);
        Task<Result<bool>> DeletePerson(int id);
        //Task AddAsync(Person person);
        //Task UpdateAsync(Person person);
    }
}