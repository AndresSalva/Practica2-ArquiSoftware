using ServicePerson.Application.Interfaces;
using ServicePerson.Domain.Entities;
using ServicePerson.Domain.Ports;

namespace ServicePerson.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public Task<Person> GetPersonById(int id) => _personRepository.GetByIdAsync(id);
        public Task<IEnumerable<Person>> GetAllPeople() => _personRepository.GetAllAsync();
        public Task<bool> DeletePerson(int id) => _personRepository.DeleteByIdAsync(id);
    }
}