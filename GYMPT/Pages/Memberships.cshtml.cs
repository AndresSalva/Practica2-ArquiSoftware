using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class MembershipsModel : PageModel
    {
        private readonly MembershipRepository _repo;

        public IEnumerable<Membership> MembershipList { get; private set; } = Enumerable.Empty<Membership>();

        public MembershipsModel(MembershipRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            var allMemberships = await _repo.GetAllAsync();
            MembershipList = allMemberships.Where(m => m.IsActive == true);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _repo.DeleteByIdAsync(id);
            return RedirectToPage();
        }
    }
}