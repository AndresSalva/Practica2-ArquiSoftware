using ServiceMembership.Domain.Entities;

namespace ServiceMembership.Domain.Ports;

public interface IMembershipRepository
{
    Task<Membership?> GetByIdAsync(int id);
    Task<IEnumerable<Membership>> GetAllAsync();
    Task<Membership> CreateAsync(Membership entity);
    Task<Membership?> UpdateAsync(Membership entity);
    Task<bool> DeleteByIdAsync(int id);
}
