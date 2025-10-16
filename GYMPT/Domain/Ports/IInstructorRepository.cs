using GYMPT.Domain.Entities;

namespace GYMPT.Domain.Ports
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<Instructor> GetByEmailAsync(string email);
        Task<bool> UpdatePasswordAsync(int id, string password);
    }
}