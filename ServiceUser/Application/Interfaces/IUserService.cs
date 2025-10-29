using ServiceUser.Domain.Entities;

namespace ServiceUser.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetInstructorById(int id);
        Task<IEnumerable<User>> GetAllInstructors();
        Task CreateNewInstructor(User newInstructor);
        Task UpdateInstructorData(User instructorToUpdate);
        Task<bool> UpdatePasswordAsync(int userId, string newHash);
    }
}