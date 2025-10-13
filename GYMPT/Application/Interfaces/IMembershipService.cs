using GYMPT.Domain.Entities;

namespace GYMPT.Application.Interfaces
{
    public interface IMembershipService
    {
        Task<Membership> GetMembershipById(int id);
        Task<IEnumerable<Membership>> GetAllMemberships();

        // ANTES: Task CreateNewMembership(Membership newMembership);
        // AHORA: Devuelve la membresía creada (con su nuevo ID)
        Task<Membership> CreateNewMembership(Membership newMembership);

        // ANTES: Task UpdateMembershipData(Membership membershipToUpdate);
        // AHORA: Devuelve true si la actualización fue exitosa
        Task<bool> UpdateMembershipData(Membership membershipToUpdate);

        // ANTES: Task DeleteMembership(int id);
        // AHORA: Devuelve true si la eliminación fue exitosa
        Task<bool> DeleteMembership(int id);
    }
}