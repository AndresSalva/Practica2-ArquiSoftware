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
        public Task<Membership> CreateNewMembership(Membership newMembership) => _membershipRepository.CreateAsync(newMembership);
        public Task<bool> DeleteMembership(int id) => _membershipRepository.DeleteByIdAsync(id);

        // ===== ESTE ES EL MÉTODO CORREGIDO =====
        public async Task<bool> UpdateMembershipData(Membership membershipToUpdate)
        {
            // 1. Llama al repositorio, que devuelve la membresía actualizada.
            var updatedMembership = await _membershipRepository.UpdateAsync(membershipToUpdate);

            // 2. Comprueba si el resultado NO es nulo.
            //    Si no es nulo, la actualización fue exitosa (devuelve true).
            //    Si es nulo, algo falló (devuelve false).
            return updatedMembership != null;
        }
    }
}