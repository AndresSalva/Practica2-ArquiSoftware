using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;

namespace GYMPT.Factories
{

    public class ClientRepositoryCreator : RepositoryCreator<Client>
    {
        public override IRepository<Client> CreateRepository()
        {
            return new ClientRepository();
        }
    }
}