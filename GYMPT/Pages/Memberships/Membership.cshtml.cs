using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Pages.Memberships
{
    public class MembershipModel : PageModel
    {
        private readonly IMembershipService _membershipService;
        public IEnumerable<Membership> MembershipList { get; set; } = new List<Membership>();

        public MembershipModel(IMembershipService membershipService)
        {
            _membershipService = membershipService;
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
                TempData["SuccessMessage"] = "La membresía ha sido eliminada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la membresía.";
            }
            return RedirectToPage();
        }
    }
}