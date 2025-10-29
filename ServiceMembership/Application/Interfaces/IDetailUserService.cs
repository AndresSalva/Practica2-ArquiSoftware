using ServiceCommon.Application;
using ServiceMembership.Domain.Entities;

namespace ServiceMembership.Application.Interfaces;

public interface IDetailUserService
{
    Task<Result<DetailsUser>> GetDetailUserById(int id);
    Task<Result<IReadOnlyCollection<DetailsUser>>> GetAllDetailUsers();
    Task<Result<DetailsUser>> CreateNewDetailUser(DetailsUser newDetailUser);
    Task<Result<DetailsUser>> UpdateDetailUserData(DetailsUser detailUserToUpdate);
    Task<Result> DeleteDetailUser(int id);
}
