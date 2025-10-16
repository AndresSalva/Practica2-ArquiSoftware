using GYMPT.Domain.Entities;

namespace GYMPT.Application.Interfaces
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