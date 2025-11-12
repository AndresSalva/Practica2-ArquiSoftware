using ServiceUser.Application.Common; 
using ServiceUser.Domain.Entities;

namespace ServiceUser.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<User>> GetUserById(int id);
        Task<IEnumerable<User>> GetAllUsers(); 
        Task<Result<User>> CreateUser(User newUser);
        Task<Result<User>> UpdateUser(User userToUpdate);
        Task<Result<bool>> DeleteUser(int userId);
        Task<Result<bool>> UpdatePassword(int userId, string newPassword);
    }
}