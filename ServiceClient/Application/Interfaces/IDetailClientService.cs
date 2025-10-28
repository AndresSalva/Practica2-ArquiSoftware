using ServiceClient.Application.Common;
using static System.Net.Mime.MediaTypeNames;
using ServiceClient.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Application.Interfaces
{
    public interface IDetailClientService
    {
        Task<Client> GetClientByIdAsync(int clientId);
        Task<IEnumerable<Client>> GetAllClientsAsync();
    }
}