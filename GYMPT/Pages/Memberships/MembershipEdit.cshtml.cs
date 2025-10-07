using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace GYMPT.Pages.Memberships
{
    public class MembershipEditModel : PageModel
    {

        private readonly IConfiguration _configuration;

        [BindProperty]
        public Membership Membership { get; set; }

        public MembershipEditModel()
        {
        }
        private IRepository<Membership> CreateMembershipRepository()
        {
            var factory = new MembershipRepositoryCreator();
            return factory.CreateRepository();
        }

        public async Task<IActionResult> OnGetAsync(short id)
        {
            var repo = CreateMembershipRepository();
            Membership = await repo.GetByIdAsync(id);

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

            var repo = CreateMembershipRepository();

            await repo.UpdateAsync(Membership);

            return RedirectToPage("/Memberships/Membership");
        }
    }
}