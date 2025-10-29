using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYMPT.Application.Facades
{
    public interface ISelectDataFacade
    {
        Task<SelectList> GetClientOptionsAsync();
        Task<SelectList> GetInstructorOptionsAsync();
        Task<SelectList> GetMembershipOptionsAsync();
    }
}
