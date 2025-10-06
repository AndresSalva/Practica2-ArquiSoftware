using GYMPT.Domain;
using System.Threading.Tasks;

namespace GYMPT.Data.Contracts
{
    public interface IInstructorRepository
    {
        Task<Instructor> GetByIdAsync(long id);
        Task CreateAsync(Instructor instructor);
        Task<bool> UpdateAsync(Instructor instructor); // <-- agregar esto
    }
}