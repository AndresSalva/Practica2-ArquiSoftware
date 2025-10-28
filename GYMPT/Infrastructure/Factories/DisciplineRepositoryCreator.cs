using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Persistence;
using ServiceUser.Domain.Ports;

namespace GYMPT.Infrastructure.Factories
{
    public class DisciplineRepositoryCreator : RepositoryCreator<Discipline>
    {
        public override IRepository<Discipline> CreateRepository()
        {
            return new DisciplineRepository();
        }
    }
}