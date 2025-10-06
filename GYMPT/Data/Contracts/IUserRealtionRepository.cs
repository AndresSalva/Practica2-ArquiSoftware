using GYMPT.Models;

namespace GYMPT.Data.Contracts
{
    public interface IUserRelationRepository<T> where T : User
    {
        Task<T> GetByIdAsync(int id);
        Task CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
    }
}