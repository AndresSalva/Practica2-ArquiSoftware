using GYMPT.Models;

namespace GYMPT.Data
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<Membership>> GetAllAsync();
    }
}