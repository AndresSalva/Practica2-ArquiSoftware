using GYMPT.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Data.Contracts
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        //Task<T> GetByIdAsync(long id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        //Task<bool> DeleteByIdAsync(long id);
    }
}