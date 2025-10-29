using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using ServiceUser.Domain.Ports;
using ServiceUser.Domain.Entities;


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
            if (typeof(T) == typeof(Person))
                return (RepositoryCreator<T>)(object)new PersonRepositoryCreator();

            if (typeof(T) == typeof(Client))
                return (    RepositoryCreator<T>)(object)new ClientRepositoryCreator();

            return null;
        }
    }
}
