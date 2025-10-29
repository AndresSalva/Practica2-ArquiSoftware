using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;

namespace ServiceUser.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _instructorRepository;

        public UserService(IUserRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public Task<User> GetInstructorById(int id) => _instructorRepository.GetByIdAsync(id);
        public Task<IEnumerable<User>> GetAllInstructors() => _instructorRepository.GetAllAsync();
        public Task CreateNewInstructor(User newInstructor) => _instructorRepository.CreateAsync(newInstructor);
        public Task UpdateInstructorData(User instructorToUpdate) => _instructorRepository.UpdateAsync(instructorToUpdate);
        private Task<bool> UpdatePassword(int id, string password) => _instructorRepository.UpdatePasswordAsync(id, password);
        public async Task<bool> UpdatePasswordAsync(int userId, string password)
        {
            var user = await GetInstructorById(userId);
            if (user == null)
            {
                return false;
            }
            return await UpdatePassword(userId, password);
        }
    }
}