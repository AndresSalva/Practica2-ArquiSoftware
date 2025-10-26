using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;

namespace GYMPT.Pages.Memberships
{
    [Authorize(Roles = "Admin")]
    public class MembershipCreateModel : PageModel
    {
        private readonly IMembershipService _membershipService;

        [BindProperty]
        public Membership Membership { get; set; } = new();

        public MembershipCreateModel(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _membershipService.CreateNewMembership(Membership);
            if (result.IsFailure)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return Page();
            }

            TempData["SuccessMessage"] = $"La membresía '{Membership.Name}' ha sido creada exitosamente.";
            return RedirectToPage("/Memberships/Membership");
        }
    }
}
