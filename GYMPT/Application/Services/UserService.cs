using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;

namespace GYMPT.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> GetUserById(int id) => _userRepository.GetByIdAsync(id);
        public Task<IEnumerable<User>> GetAllUsers() => _userRepository.GetAllAsync();

        // ANTES (incorrecto):
        // public Task DeleteUser(int id) => _userRepository.DeleteByIdAsync(id);

        // AHORA (CORREGIDO):
        // La clase ahora devuelve el booleano que recibe del repositorio.
        public Task<bool> DeleteUser(int id) => _userRepository.DeleteByIdAsync(id);
    }
}