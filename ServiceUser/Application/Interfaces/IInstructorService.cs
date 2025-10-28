using ServiceUser.Domain.Entities;

namespace ServiceUser.Application.Interfaces
{
    public interface IInstructorService
    {
        Task<Instructor> GetInstructorById(int id);
        Task<IEnumerable<Instructor>> GetAllInstructors();
        Task CreateNewInstructor(Instructor newInstructor);
        Task UpdateInstructorData(Instructor instructorToUpdate);
        Task<bool> UpdatePasswordAsync(int userId, string newHash);
    }
}