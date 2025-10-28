using GYMPT.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Application.Interfaces
{
    public interface IDetailUserService
    {
        // --- SE ESTANDARIZAN LOS NOMBRES DE LOS MÉTODOS ---
        Task<IEnumerable<DetailsUser>> GetAllAsync();
        Task<DetailsUser?> GetByIdAsync(int id);
        Task<DetailsUser> CreateAsync(DetailsUser detailUser);
        Task<DetailsUser?> UpdateAsync(DetailsUser detailUser);
        Task<bool> DeleteByIdAsync(int id);
    }
}