using ServiceClient.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetByIdAsync(int id);

        // --- AÑADE ESTA LÍNEA AL CONTRATO ---
        Task<IEnumerable<User>> GetAllAsync();

        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(User user);
        Task<bool> DeleteByIdAsync(int id);
    }
}