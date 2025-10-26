using System.Collections.Generic;
using ServiceMembership.Application.Common;
using ServiceMembership.Domain.Entities;

namespace ServiceMembership.Application.Interfaces;

public interface IMembershipService
{
    Task<Result<Membership>> GetMembershipById(int id);
    Task<Result<IReadOnlyCollection<Membership>>> GetAllMemberships();
    Task<Result<Membership>> CreateNewMembership(Membership newMembership);
    Task<Result<Membership>> UpdateMembershipData(Membership membershipToUpdate);
    Task<Result> DeleteMembership(int id);
}
