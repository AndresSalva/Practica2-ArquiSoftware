using GYMPT.Models;
using Supabase.Postgrest.Models;

namespace GYMPT.Data.Contracts
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllAsync();

    }
}