using GYMPT.Domain.Entities;

namespace GYMPT.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers();
        Task DeleteUser(int id);
    }
}