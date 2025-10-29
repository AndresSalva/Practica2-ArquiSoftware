// Ruta: ServiceClient/Domain/Ports/IUserRepository.cs (o donde la tengas ubicada)
using ServiceClient.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Domain.Ports
{
    public interface IUserRepository
    {
        // Métodos para leer datos
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);

        // Métodos para escribir datos (aquí corregimos los nombres)
        Task<User> CreateAsync(User user);      // Usaremos 'CreateAsync' en lugar de 'AddAsync'
        Task<User?> UpdateAsync(User user);     // Contrato para actualizar
        Task<bool> DeleteByIdAsync(int id);   // Usaremos 'DeleteByIdAsync' para ser más explícitos
    }
}