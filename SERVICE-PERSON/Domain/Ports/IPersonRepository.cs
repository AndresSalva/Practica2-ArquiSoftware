using ServicePerson.Domain.Entities;
using ServiceCommon.Domain.Ports;

namespace ServicePerson.Domain.Ports
{
    public interface IPersonRepository : IRepository<Person>
    {
    }
}