using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Memberships
{
    public class MembershipModel : PageModel
    {
        private readonly MembershipRepository _repo;

        public IEnumerable<Membership> MembershipList { get; private set; } = Enumerable.Empty<Membership>();

        public MembershipModel(MembershipRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            var allMemberships = await _repo.GetAllAsync();
            MembershipList = allMemberships.Where(m => m.IsActive == true);
        }

        public async Task<IActionResult> OnPostDeleteAsync(short id)
        {
            await _repo.DeleteByIdAsync(id);
            return RedirectToPage();
        }
    }
}