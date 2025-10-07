using GYMPT.Data.Contracts;
using GYMPT.Factories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Memberships
{
    public class MembershipModel : PageModel
    {

        public IEnumerable<Membership> MembershipList { get; private set; } = Enumerable.Empty<Membership>();

        public MembershipModel()
        {
        }
        private IRepository<Membership> CreateMembershipRepository()
        {
            var factory = new MembershipRepositoryCreator();
            return factory.CreateRepository();
        }

        public async Task OnGetAsync()
        {
            var repo = CreateMembershipRepository();
            var allMemberships = await repo.GetAllAsync();
            MembershipList = allMemberships.Where(m => m.IsActive == true);
        }

        public async Task<IActionResult> OnPostDeleteAsync(short id)
        {
            var repo = CreateMembershipRepository();
            await repo.DeleteByIdAsync(id);
            return RedirectToPage();
        }
    }
}