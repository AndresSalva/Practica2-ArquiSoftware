using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;

namespace GYMPT.Factories
{
    public class DisciplineRepositoryCreator : RepositoryCreator<Discipline>
    {
        public override IRepository<Discipline> CreateRepository()
        {
            return new DisciplineRepository();
        }
    }
}