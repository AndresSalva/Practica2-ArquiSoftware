using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using ServiceClient.Domain.Entities;
using ServiceDiscipline.Domain.Entities;
using ServiceMembership.Domain.Entities;
// Ya no necesitamos usings de ServiceClient aqu�, porque esta f�brica ya no lo conoce.

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
            // --- L�gica para User y Client ELIMINADA ---
            // La creaci�n de UserRepository y ClientRepository ahora es responsabilidad
            // exclusiva del m�dulo ServiceClient, a trav�s de la inyecci�n de dependencias.

            if (typeof(T) == typeof(Instructor))
                return (RepositoryCreator<T>)(object)new InstructorRepositoryCreator();

            if (typeof(T) == typeof(User))
                return (RepositoryCreator<T>)(object)new UseRep();

            if (typeof(T) == typeof(Client))
                return (RepositoryCreator<T>)(object)new ClientRepositoryCreator();

            // if (typeof(T) == typeof(Discipline))
            //     return (RepositoryCreator<T>)(object)new DisciplineRepositoryCreato();
            // if (typeof(T) == typeof(Membership))
            //     return (RepositoryCreator<T>)(object)new MembershipRepositoryCreator();
            if (typeof(T) == typeof(Discipline))
                return (RepositoryCreator<T>)(object)new DisciplineRepositoryCreator();

            // if (typeof(T) == typeof(DetailsUser))
            //     return (RepositoryCreator<T>)(object)new DetailUserRepositoryCreator();

            return null;
        }
    }
}