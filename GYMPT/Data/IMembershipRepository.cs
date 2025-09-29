using GYMPT.Models;
using Supabase.Postgrest.Models;

namespace GYMPT.Data
{
    public interface IMembershipRepository<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}