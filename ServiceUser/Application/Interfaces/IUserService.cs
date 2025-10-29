using ServiceUser.Domain.Entities;

namespace ServiceUser.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserById(int id); // ahora devuelve cualquier User (Instructor/Admin)
        Task<IEnumerable<User>> GetAllUsers(); // devuelve todos los usuarios con roles
        Task CreateUser(User newUser);
        Task UpdateUser(User userToUpdate);
        Task<bool> DeleteUser(int userId); // nuevo
        Task<bool> UpdatePasswordAsync(int userId, string newHash);
    }
}
