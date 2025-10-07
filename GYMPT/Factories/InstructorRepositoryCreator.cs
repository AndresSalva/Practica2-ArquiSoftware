using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;

namespace GYMPT.Factories
{
    public class InstructorRepositoryCreator : RepositoryCreator<Instructor>
    {
        public override IRepository<Instructor> CreateRepository()
        {
            return new InstructorRepository();
        }
    }
}