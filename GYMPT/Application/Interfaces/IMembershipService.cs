using GYMPT.Domain.Entities;

namespace GYMPT.Application.Interfaces
{
    public interface IMembershipService
    {
        Task<Membership> GetMembershipById(int id);
        Task<IEnumerable<Membership>> GetAllMemberships();
        Task CreateNewMembership(Membership newMembership);
        Task UpdateMembershipData(Membership membershipToUpdate);
        Task DeleteMembership(int id);
    }
}