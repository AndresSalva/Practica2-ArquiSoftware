using GYMPT.Domain.Entities;

namespace GYMPT.Application.Interfaces
{
    public interface IDetailUserService
    {
        Task<DetailsUser> GetDetailUserById(int id);
        Task<IEnumerable<DetailsUser>> GetAllDetailUsers();
        Task CreateNewDetailUser(DetailsUser newDetailUser);
        Task UpdateDetailUserData(DetailsUser detailUserToUpdate);
        Task DeleteDetailUser(int id);
    }
}