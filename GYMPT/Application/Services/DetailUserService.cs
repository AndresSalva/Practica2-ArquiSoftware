using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports; // Asegúrate que este sea el namespace para IDetailUserRepository
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Application.Services
{
    public class DetailUserService : IDetailUserService
    {
        private readonly IDetailUserRepository _detailUserRepository;

        public DetailUserService(IDetailUserRepository detailUserRepository)
        {
            _detailUserRepository = detailUserRepository;
        }

        // --- LOS MÉTODOS AHORA IMPLEMENTAN EL CONTRATO ESTANDARIZADO ---

        public async Task<DetailsUser> CreateAsync(DetailsUser detailUser)
        {
            // La lógica del servicio es delegar la llamada al repositorio
            // Asumiendo que el repositorio también usa el nombre estándar CreateAsync
            return await _detailUserRepository.CreateAsync(detailUser);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await _detailUserRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<DetailsUser>> GetAllAsync()
        {
            return await _detailUserRepository.GetAllAsync();
        }

        public async Task<DetailsUser?> GetByIdAsync(int id)
        {
            return await _detailUserRepository.GetByIdAsync(id);
        }

        public async Task<DetailsUser?> UpdateAsync(DetailsUser detailUser)
        {
            return await _detailUserRepository.UpdateAsync(detailUser);
        }
    }
}