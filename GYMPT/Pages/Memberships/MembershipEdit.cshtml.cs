using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Memberships
{
    public class MembershipEditModel : PageModel
    {
        private readonly IMembershipService _membershipService;

        [BindProperty]
        public Membership Membership { get; set; }

        public MembershipEditModel(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public async Task<IActionResult> OnGetAsync(int id) // Cambiado a int para consistencia
        {
            Membership = await _membershipService.GetMembershipById(id);

            if (Membership == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _membershipService.UpdateMembershipData(Membership);
            TempData["SuccessMessage"] = $"Membresía '{Membership.Name}' actualizada correctamente.";
            return RedirectToPage("/Memberships/Membership");
        }
    }
}