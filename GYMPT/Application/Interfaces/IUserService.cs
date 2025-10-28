using GYMPT.Domain.Entities;
using ServiceUser.Domain.Entities;

namespace GYMPT.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<bool> DeleteUser(int id);
    }
}