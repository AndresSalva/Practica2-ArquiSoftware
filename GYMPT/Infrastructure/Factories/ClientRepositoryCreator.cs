using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Persistence;

namespace GYMPT.Infrastructure.Factories
{

    public class ClientRepositoryCreator : RepositoryCreator<Client>
    {
        public override IRepository<Client> CreateRepository()
        {
            return new ClientRepository();
        }
    }
}