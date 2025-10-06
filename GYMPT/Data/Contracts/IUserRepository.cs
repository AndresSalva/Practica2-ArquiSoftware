using GYMPT.Domain;
using System.Threading.Tasks;
using GYMPT.Models;
using System.Collections.Generic;

namespace GYMPT.Data.Contracts
{
    public interface IUserRepository : IRepository<UserData>
    {
        Task<UserData> GetByIdAsync(long id);
        Task<long> CreateAsync(UserData entity);
        Task<bool> UpdateAsync(UserData entity);
        Task<bool> DeleteAsync(long id);
    }
}
