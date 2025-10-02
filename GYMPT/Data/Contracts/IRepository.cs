using GYMPT.Models;
using Supabase.Postgrest.Models;

namespace GYMPT.Data.Contracts
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}