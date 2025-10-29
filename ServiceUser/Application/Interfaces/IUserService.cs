using ServiceUser.Domain.Entities;

namespace ServiceUser.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers();
        Task CreateUser(User newUser);
        Task UpdateUser(User userToUpdate);
        Task<bool> DeleteUser(int userId);
        Task<bool> UpdatePasswordAsync(int userId, string newHash);
    }
}
