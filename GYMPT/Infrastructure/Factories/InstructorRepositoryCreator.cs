using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Persistence;

namespace GYMPT.Infrastructure.Factories
{
    public class InstructorRepositoryCreator : RepositoryCreator<Instructor>
    {
        public override IRepository<Instructor> CreateRepository()
        {
            return new InstructorRepository();
        }
    }
}