using GYMPT.Infrastructure.Persistence;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;

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