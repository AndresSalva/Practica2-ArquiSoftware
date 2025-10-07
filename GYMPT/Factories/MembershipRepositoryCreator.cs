using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;

namespace GYMPT.Factories
{

    public class MembershipRepositoryCreator : RepositoryCreator<Membership>
    {
        public override IRepository<Membership> CreateRepository()
        {
            return new MembershipRepository();
        }
    }
}