using GYMPT.Domain;
using System.Threading.Tasks;

namespace GYMPT.Data.Contracts
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(long id);
        Task CreateAsync(Client client);
        Task<bool> UpdateAsync(Client client);
    }
}
