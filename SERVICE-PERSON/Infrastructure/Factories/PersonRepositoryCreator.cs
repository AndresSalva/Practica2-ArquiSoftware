using ServicePerson.Infrastructure.Persistence;
using ServicePerson.Domain.Entities;
using ServicePerson.Domain.Ports;

namespace ServicePerson.Infraestructure.Factories
{
    public class PersonRepositoryCreator : RepositoryCreator<Person>
    {
        public override IRepository<Person> CreateRepository()
        {
            return new PersonRepository();
        }
    }
}