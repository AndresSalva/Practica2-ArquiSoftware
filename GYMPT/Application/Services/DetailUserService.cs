using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;

namespace GYMPT.Application.Services
{
    public class DetailUserService : IDetailUserService
    {
        private readonly IDetailUserRepository _detailUserRepository;

        public DetailUserService(IDetailUserRepository detailUserRepository)
        {
            _detailUserRepository = detailUserRepository;
        }

        public Task<DetailsUser> GetDetailUserById(int id) => _detailUserRepository.GetByIdAsync(id);
        public Task<IEnumerable<DetailsUser>> GetAllDetailUsers() => _detailUserRepository.GetAllAsync();
        public Task CreateNewDetailUser(DetailsUser newDetailUser) => _detailUserRepository.CreateAsync(newDetailUser);
        public Task UpdateDetailUserData(DetailsUser detailUserToUpdate) => _detailUserRepository.UpdateAsync(detailUserToUpdate);
        public Task DeleteDetailUser(int id) => _detailUserRepository.DeleteByIdAsync(id);
    }
}