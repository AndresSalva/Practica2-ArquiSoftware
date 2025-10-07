using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;

namespace GYMPT.Factories
{

    public class DetailUserRepositoryCreator : RepositoryCreator<DetailsUser>
    {
        public override IRepository<DetailsUser> CreateRepository()
        {
            return new DetailUserRepository();
        }
    }
}