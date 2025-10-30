using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMPT.Application.Facades
{
    public interface ISelectDataFacade
    {
        Task<SelectList> GetClientOptionsAsync();
        Task<SelectList> GetInstructorOptionsAsync();
        Task<SelectList> GetMembershipOptionsAsync();
    }
}
