using ServiceCommon.Domain.Ports;
using ServiceUser.Domain.Entities;

namespace ServiceUser.Domain.Ports
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> CreateAsync(User entity);
        Task<User> UpdateAsync(User entity);
        Task<bool> UpdatePasswordAsync(int id, string password);
    }
}