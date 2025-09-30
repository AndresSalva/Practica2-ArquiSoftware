using GYMPT.Domain;
using System.Threading.Tasks;

namespace GYMPT.Data.Contracts
{
    public interface IInstructorRepository
    {
        Task<Instructor> GetByIdAsync(long id);
    }
}