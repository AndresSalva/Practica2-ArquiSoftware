using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;

namespace ServiceUser.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public Task<Instructor> GetInstructorById(int id) => _instructorRepository.GetByIdAsync(id);
        public Task<IEnumerable<Instructor>> GetAllInstructors() => _instructorRepository.GetAllAsync();
        public Task CreateNewInstructor(Instructor newInstructor) => _instructorRepository.CreateAsync(newInstructor);
        public Task UpdateInstructorData(Instructor instructorToUpdate) => _instructorRepository.UpdateAsync(instructorToUpdate);
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