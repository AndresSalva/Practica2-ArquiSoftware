using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Memberships
{
    public class MembershipCreateModel : PageModel
    {

        [BindProperty]
        public Membership Membership { get; set; } = new();

        public MembershipCreateModel()
        {
        }
        private IRepository<Membership> CreateMembershipRepository()
        {
            var factory = new MembershipRepositoryCreator();
            return factory.CreateRepository();
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var repo = CreateMembershipRepository();
            await repo.CreateAsync(Membership);

            return RedirectToPage("/Memberships/Membership");
        }
    }
}