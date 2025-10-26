using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;
using ServiceMembership.Domain.Ports;

namespace ServiceMembership.Application.Services;

public class MembershipService : IMembershipService
{
    private readonly IMembershipRepository _membershipRepository;

    public MembershipService(IMembershipRepository membershipRepository)
    {
        _membershipRepository = membershipRepository;
    }

    public Task<Membership?> GetMembershipById(int id) => _membershipRepository.GetByIdAsync(id);

    public Task<IEnumerable<Membership>> GetAllMemberships() => _membershipRepository.GetAllAsync();

    public Task<Membership> CreateNewMembership(Membership newMembership) => _membershipRepository.CreateAsync(newMembership);

    public async Task<bool> UpdateMembershipData(Membership membershipToUpdate)
    {
        var updatedMembership = await _membershipRepository.UpdateAsync(membershipToUpdate);
        return updatedMembership is not null;
    }

    public Task<bool> DeleteMembership(int id) => _membershipRepository.DeleteByIdAsync(id);
}
