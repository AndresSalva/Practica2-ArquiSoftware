using ServiceMembership.Domain.Entities;

namespace ServiceMembership.Application.Interfaces;

public interface IMembershipService
{
    Task<Membership?> GetMembershipById(int id);
    Task<IEnumerable<Membership>> GetAllMemberships();
    Task<Membership> CreateNewMembership(Membership newMembership);
    Task<bool> UpdateMembershipData(Membership membershipToUpdate);
    Task<bool> DeleteMembership(int id);
}
