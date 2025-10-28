using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
// Ya no necesitamos usings de ServiceClient aquí, porque esta fábrica ya no lo conoce.

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
            // --- Lógica para User y Client ELIMINADA ---
            // La creación de UserRepository y ClientRepository ahora es responsabilidad
            // exclusiva del módulo ServiceClient, a través de la inyección de dependencias.

            if (typeof(T) == typeof(Instructor))
                return (RepositoryCreator<T>)(object)new InstructorRepositoryCreator();

            if (typeof(T) == typeof(Discipline))
                return (RepositoryCreator<T>)(object)new DisciplineRepositoryCreator();

            if (typeof(T) == typeof(Membership))
                return (RepositoryCreator<T>)(object)new MembershipRepositoryCreator();

            if (typeof(T) == typeof(DetailsUser))
                return (RepositoryCreator<T>)(object)new DetailUserRepositoryCreator();

            return null;
        }
    }
}