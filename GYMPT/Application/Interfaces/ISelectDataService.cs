using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GYMPT.Application.Interfaces
{
    public interface ISelectDataService
    {
        Task<SelectList> GetUserOptionsAsync();
        Task<SelectList> GetMembershipOptionsAsync();
        Task<SelectList> GetDisciplineOptionsAsync();
        Task<SelectList> GetInstructorOptionsAsync(); 
    }
}
