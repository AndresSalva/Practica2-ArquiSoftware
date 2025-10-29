using GYMPT.Domain.Entities;
using ServiceUser.Domain.Entities;

namespace GYMPT.Application.Interfaces
{
    public interface IPersonService
    {
        Task<Person> GetUserById(int id);
        Task<IEnumerable<Person>> GetAllUsers();
        Task<bool> DeleteUser(int id);
    }
}