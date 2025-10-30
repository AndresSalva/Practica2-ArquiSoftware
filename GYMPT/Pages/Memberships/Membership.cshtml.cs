using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceCommon.Infrastructure.Services;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;

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
            var result = await _membershipService.GetAllMemberships();
            if (result.IsFailure || result.Value is null)
            {
                TempData["ErrorMessage"] = result.Error ?? "No se pudo obtener el listado de membresías.";
                MembershipList = Enumerable.Empty<Membership>();
                return;
            }

            MembershipList = result.Value;
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _membershipService.DeleteMembership(id);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "La membresía ha sido eliminada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error ?? "No se pudo eliminar la membresía.";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostEditAsync(int id)
        {
            string token = _urlTokenSingleton.Protect(id.ToString());
            return RedirectToPage("./MembershipEdit", new { token });
        }
    }
}
