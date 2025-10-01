using GYMPT.Models;
using Supabase.Postgrest.Models;

namespace GYMPT.Data.Contracts
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}