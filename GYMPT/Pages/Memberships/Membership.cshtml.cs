using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Memberships
{
    [Authorize(Roles = "Admin")]
    public class MembershipModel : PageModel
    {
        private readonly IMembershipService _membershipService;
        private readonly ParameterProtector _urlTokenSingleton;
        public IEnumerable<Membership> MembershipList { get; set; } = new List<Membership>();

        public MembershipModel(IMembershipService membershipService, ParameterProtector urlTokenSingleton)
        {
            _membershipService = membershipService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            MembershipList = await _membershipService.GetAllMemberships();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var success = await _membershipService.DeleteMembership(id);
            if (success)
            {
                TempData["SuccessMessage"] = "La membres�a ha sido eliminada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la membres�a.";
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            // Generate a route token using the UrlTokenSingleton and redirect to the edit page
            string token = _urlTokenSingleton.Protect(id.ToString());
            return RedirectToPage("./MembershipEdit", new { token });
        }
    }
}