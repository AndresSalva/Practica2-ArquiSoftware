using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;

namespace GYMPT.Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;

        public MembershipService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public Task<Membership> GetMembershipById(int id) => _membershipRepository.GetByIdAsync(id);
        public Task<IEnumerable<Membership>> GetAllMemberships() => _membershipRepository.GetAllAsync();
        public Task CreateNewMembership(Membership newMembership) => _membershipRepository.CreateAsync(newMembership);
        public Task UpdateMembershipData(Membership membershipToUpdate) => _membershipRepository.UpdateAsync(membershipToUpdate);
        public Task DeleteMembership(int id) => _membershipRepository.DeleteByIdAsync(id);
    }
}