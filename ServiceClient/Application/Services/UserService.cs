using Microsoft.Extensions.Logging;
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // --- AÑADE LA IMPLEMENTACIÓN DEL NUEVO MÉTODO ---
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los usuarios");
            // La lógica del servicio es delegar la llamada al repositorio.
            return await _userRepository.GetAllAsync();
        }

        // --- MÉTODOS EXISTENTES ---
        public async Task<User?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo usuario con Id {UserId}", id);
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            _logger.LogInformation("Creando usuario {UserName}", user.Name);
            return await _userRepository.CreateAsync(user);
        }

        public async Task<User?> UpdateAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user, nameof(user));
            _logger.LogInformation("Actualizando usuario con Id {UserId}", user.Id);
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            _logger.LogInformation("Eliminando usuario con Id {UserId}", id);
            return await _userRepository.DeleteByIdAsync(id);
        }
    }
}