using GYMPT.Infrastructure.Persistence;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;

namespace GYMPT.Infrastructure.Factories
{
    public class UserRepositoryCreator : RepositoryCreator<User>
    {
        public override IRepository<User> CreateRepository()
        {
            return new UserRepository();
        }
    }
}