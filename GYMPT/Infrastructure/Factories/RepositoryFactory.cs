using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;

namespace GYMPT.Infrastructure.Factories
{
    public class RepositoryFactory
    {
        public IRepository<T> CreateRepository<T>() where T : class
        {
            RepositoryCreator<T>? creator = GetCreator<T>();

            if (creator == null)
                throw new InvalidOperationException($"No repository creator found for type {typeof(T).Name}");

            return creator.CreateRepository();
        }

        private RepositoryCreator<T>? GetCreator<T>() where T : class
        {
            if (typeof(T) == typeof(Instructor))
                return (RepositoryCreator<T>)(object)new InstructorRepositoryCreator();

            if (typeof(T) == typeof(User))
                return (RepositoryCreator<T>)(object)new UserRepositoryCreator();

            if (typeof(T) == typeof(Client))
                return (RepositoryCreator<T>)(object)new ClientRepositoryCreator();

            if (typeof(T) == typeof(Discipline))
                return (RepositoryCreator<T>)(object)new DisciplineRepositoryCreator();

            return null;
        }
    }
}
