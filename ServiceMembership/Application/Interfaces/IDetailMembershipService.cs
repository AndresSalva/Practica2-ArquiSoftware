using ServiceCommon.Application;
using ServiceMembership.Domain.Entities;

namespace ServiceMembership.Application.Interfaces;

public interface IDetailMembershipService
{
    Task<Result<IReadOnlyCollection<DetailsMembership>>> GetAllDetailsMemberships();
    Task<Result<IReadOnlyCollection<DetailsMembership>>> GetDetailsMembershipsByMembership(short membershipId);
    Task<Result<DetailsMembership>> GetDetailsMembership(short membershipId, short disciplineId);
    Task<Result<DetailsMembership>> CreateDetailsMembership(DetailsMembership newDetailsMembership);
    Task<Result<DetailsMembership>> UpdateDetailsMembership(short membershipId, short disciplineId, DetailsMembership updatedDetailsMembership);
    Task<Result> DeleteDetailsMembership(short membershipId, short disciplineId);
    Task<Result> DeleteDetailsForMembership(short membershipId);
    Task<Result> UpdateDisciplinesForMembership(short membershipId, IEnumerable<short> disciplineIds);
}
