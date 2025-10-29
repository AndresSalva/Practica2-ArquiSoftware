using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;

namespace ServiceUser.Application.Services
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

        public Task CreateUser(User newUser) => _userRepository.CreateAsync(newUser);

        public Task UpdateUser(User userToUpdate) => _userRepository.UpdateAsync(userToUpdate);

        public Task<bool> DeleteUser(int userId) => _userRepository.DeleteByIdAsync(userId);

        private Task<bool> UpdatePassword(int id, string password) => _userRepository.UpdatePasswordAsync(id, password);

        public async Task<bool> UpdatePasswordAsync(int userId, string password)
        {
            var user = await GetUserById(userId);
            if (user == null) return false;
            return await UpdatePassword(userId, password);
        }
    }
}
