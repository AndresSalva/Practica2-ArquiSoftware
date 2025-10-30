using ServicePerson.Domain.Entities;
using ServiceCommon.Domain.Ports;

namespace ServicePerson.Domain.Ports
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(int id);
        Task<Person> CreateAsync(Person entity);
        Task<Person> UpdateAsync(Person entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}