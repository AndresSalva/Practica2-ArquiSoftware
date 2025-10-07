using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;

namespace GYMPT.Factories
{
    public class UserRepositoryCreator : RepositoryCreator<User>
    {
        public override IRepository<User> CreateRepository()
        {
            return new UserRepository();
        }
    }
}