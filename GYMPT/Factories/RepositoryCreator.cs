using GYMPT.Data.Contracts;

namespace GYMPT.Factories
{

    public abstract class RepositoryCreator<T> where T : class
    {
        public abstract IRepository<T> CreateRepository();

        public async Task<string> GetRepositoryStatus()
        {

            var repository = CreateRepository();

            var entities = await repository.GetAllAsync();
            int count = entities.Count();

            return $"El repositorio para '{typeof(T).Name}' está funcionando y reporta {count} entidades activas.";
        }
    }
}