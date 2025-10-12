using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.Memberships
{
    public class MembershipModel : PageModel
    {
        private readonly IMembershipService _membershipService;
        public IEnumerable<Membership> MembershipList { get; private set; } = Enumerable.Empty<Membership>();

        public MembershipModel(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public async Task OnGetAsync()
        {
            var allMemberships = await _membershipService.GetAllMemberships();
            MembershipList = allMemberships.Where(m => m.IsActive);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id) // Cambiado a int para consistencia
        {
            await _membershipService.DeleteMembership(id);
            TempData["SuccessMessage"] = "Membresía eliminada correctamente.";
            return RedirectToPage();
        }
    }
}