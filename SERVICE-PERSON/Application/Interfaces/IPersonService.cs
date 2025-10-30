using ServicePerson.Application.Common;
using ServicePerson.Domain.Entities;

namespace ServicePerson.Application.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllPeople();
        Task<Result<Person>> GetPersonById(int id);
        Task<Person> CreateNewPerson(Person newPerson);
        Task<Person> UpdatePerson(Person personToUpdate);
        Task<Result<bool>> DeletePerson(int id);
    }
}