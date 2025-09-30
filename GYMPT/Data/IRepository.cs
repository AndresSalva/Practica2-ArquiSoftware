using GYMPT.Models;
using Supabase.Postgrest.Models;

namespace GYMPT.Data
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T> CreateAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<bool> DeleteByIdAsync(int id); // El delete solo es logico, se debe hacer un update que modifique solamente el atributo is_active a false
    }
}