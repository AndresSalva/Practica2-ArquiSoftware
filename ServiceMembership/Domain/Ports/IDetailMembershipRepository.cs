using ServiceMembership.Domain.Entities;

namespace ServiceMembership.Domain.Ports;

public interface IDetailMembershipRepository
{
    Task<IEnumerable<DetailsMembership>> GetAllAsync();
    Task<IEnumerable<DetailsMembership>> GetByMembershipIdAsync(short membershipId);
    Task<DetailsMembership?> GetByIdsAsync(short membershipId, short disciplineId);
    Task<DetailsMembership> CreateAsync(DetailsMembership entity);
    Task<DetailsMembership?> UpdateAsync(short membershipId, short disciplineId, DetailsMembership entity);
    Task<bool> DeleteByIdsAsync(short membershipId, short disciplineId);
    Task<bool> DeleteByMembershipIdAsync(short membershipId);
}
