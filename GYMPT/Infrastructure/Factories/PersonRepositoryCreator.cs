using GYMPT.Infrastructure.Persistence;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;

namespace GYMPT.Infrastructure.Factories
{
    public class PersonRepositoryCreator : RepositoryCreator<Person>
    {
        public override IRepository<Person> CreateRepository()
        {
            return new PersonRepository();
        }
    }
}