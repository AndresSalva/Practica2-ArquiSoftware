using GYMPT.Domain.Ports;

namespace GYMPT.Infrastructure.Factories
{

    public abstract class RepositoryCreator<T> where T : class
    {
        public abstract IRepository<T> CreateRepository();

        public async Task<string> GetRepositoryStatus()
        {

            var repository = CreateRepository();

            var entities = await repository.GetAllAsync();
            int count = entities.Count();

            return $"The repository for '{typeof(T).Name}' is working and reporting {count} active entities.";
        }
    }
}