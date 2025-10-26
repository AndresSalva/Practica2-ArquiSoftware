using ServiceMembership.Domain.Entities;

namespace ServiceMembership.Application.Interfaces;

public interface IDetailUserService
{
    Task<DetailsUser?> GetDetailUserById(int id);
    Task<IEnumerable<DetailsUser>> GetAllDetailUsers();
    Task<DetailsUser> CreateNewDetailUser(DetailsUser newDetailUser);
    Task<bool> UpdateDetailUserData(DetailsUser detailUserToUpdate);
    Task<bool> DeleteDetailUser(int id);
}
