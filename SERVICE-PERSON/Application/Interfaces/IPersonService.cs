using System;
using ServicePerson.Domain.Entities;


namespace ServicePerson.Application.Interfaces
{
    public interface IPersonService
    {
        Task<Person> GetPersonById(int id);
        Task<IEnumerable<Person>> GetAllPeople();
        Task<bool> DeletePerson(int id);
    }
}