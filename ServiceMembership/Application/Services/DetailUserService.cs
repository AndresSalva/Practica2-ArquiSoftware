using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;
using ServiceMembership.Domain.Ports;

namespace ServiceMembership.Application.Services;

public class DetailUserService : IDetailUserService
{
    private readonly IDetailUserRepository _detailUserRepository;

    public DetailUserService(IDetailUserRepository detailUserRepository)
    {
        _detailUserRepository = detailUserRepository;
    }

    public Task<DetailsUser?> GetDetailUserById(int id) => _detailUserRepository.GetByIdAsync(id);

    public Task<IEnumerable<DetailsUser>> GetAllDetailUsers() => _detailUserRepository.GetAllAsync();

    public Task<DetailsUser> CreateNewDetailUser(DetailsUser newDetailUser) => _detailUserRepository.CreateAsync(newDetailUser);

    public async Task<bool> UpdateDetailUserData(DetailsUser detailUserToUpdate)
    {
        var updated = await _detailUserRepository.UpdateAsync(detailUserToUpdate);
        return updated is not null;
    }

    public Task<bool> DeleteDetailUser(int id) => _detailUserRepository.DeleteByIdAsync(id);
}
