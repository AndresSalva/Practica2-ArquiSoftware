using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
namespace GYMPT.Pages.Memberships
{
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

            await _membershipService.CreateNewMembership(Membership);
            TempData["SuccessMessage"] = $"Membresía '{Membership.Name}' creada exitosamente.";
            return RedirectToPage("/Memberships/Membership");
        }
    }
}