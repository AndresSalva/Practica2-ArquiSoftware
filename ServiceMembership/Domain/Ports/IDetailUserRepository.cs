using ServiceMembership.Domain.Entities;

namespace ServiceMembership.Domain.Ports;

public interface IDetailUserRepository
{
    Task<DetailsUser?> GetByIdAsync(int id);
    Task<IEnumerable<DetailsUser>> GetAllAsync();
    Task<DetailsUser> CreateAsync(DetailsUser entity);
    Task<DetailsUser?> UpdateAsync(DetailsUser entity);
    Task<bool> DeleteByIdAsync(int id);
}
