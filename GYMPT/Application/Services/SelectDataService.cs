using GYMPT.Application.Facades;
using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMPT.Application.Services
{
    public class SelectDataService : ISelectDataService
    {
        private readonly ISelectDataFacade _facade;

        public SelectDataService(ISelectDataFacade facade)
        {
            _facade = facade;
        }

        public async Task<SelectList> GetUserOptionsAsync()
        {
            // Ahora representa los CLIENTES (antes "users")
            return await _facade.GetClientOptionsAsync();
        }

        public async Task<SelectList> GetMembershipOptionsAsync()
        {
            return await _facade.GetMembershipOptionsAsync();
        }

        public async Task<SelectList> GetInstructorOptionsAsync()
        {
            return await _facade.GetInstructorOptionsAsync();
        }
    }
}
