using ServiceClient.Domain.Entities; // Necesario para poder usar la clase 'User'

namespace ServiceClient.Domain.Ports
{
    // 1. Cambiado de 'internal' a 'public'
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);

        Task<IEnumerable<User>> GetAllAsync();

        Task<User> CreateAsync(User entity);

        Task<User?> UpdateAsync(User entity);

        Task<bool> DeleteByIdAsync(int id);
    }
}