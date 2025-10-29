using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using ServiceUser.Domain.Entities;
using ServiceUser.Infrastructure.Persistence;


namespace GYMPT.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _userRepository;

        public PersonService(IPersonRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<Person> GetUserById(int id) => _userRepository.GetByIdAsync(id);
        public Task<IEnumerable<Person>> GetAllUsers() => _userRepository.GetAllAsync();
        public Task<bool> DeleteUser(int id) => _userRepository.DeleteByIdAsync(id);
    }
}