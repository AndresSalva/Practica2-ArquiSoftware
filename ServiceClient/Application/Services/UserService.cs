// Ruta: ServiceClient/Application/Services/UserService.cs
using ServiceClient.Application.Interfaces;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // ... GetAllAsync y GetByIdAsync no cambian ...

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> CreateAsync(User user)
        {
            user.CreatedAt = System.DateTime.UtcNow;
            user.IsActive = true;
            // CORRECCIÓN: Llamamos a CreateAsync, como se define en la interfaz
            return await _userRepository.CreateAsync(user);
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existingUser = await _userRepository.GetByIdAsync(user.Id);
            if (existingUser == null) return null;

            // ... lógica de mapeo ...
            existingUser.LastModification = System.DateTime.UtcNow;

            return await _userRepository.UpdateAsync(existingUser);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            // CORRECCIÓN: Llamamos a DeleteByIdAsync, como se define en la interfaz
            return await _userRepository.DeleteByIdAsync(id);
        }
    }
}