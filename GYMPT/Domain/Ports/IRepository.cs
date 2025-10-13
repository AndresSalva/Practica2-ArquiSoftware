using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;

namespace GYMPT.Domain.Ports
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id); 
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteByIdAsync(int id); 
    }
}

public interface IUserRepository : IRepository<User> { }
public interface IClientRepository : IRepository<Client> { }
public interface IInstructorRepository : IRepository<Instructor> { }