using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;

namespace ServiceUser.Domain.Ports
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        Task<Instructor> GetByEmailAsync(string email);
        Task<bool> UpdatePasswordAsync(int id, string password);
    }
}