using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using ServiceUser.Domain.Entities;
using ServiceUser.Infrastructure.Persistence;


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
        public Task<bool> DeleteUser(int id) => _userRepository.DeleteByIdAsync(id);
    }
}