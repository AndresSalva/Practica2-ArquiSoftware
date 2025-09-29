using GYMPT.Models;
using Supabase.Postgrest.Models;

namespace GYMPT.Data
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}