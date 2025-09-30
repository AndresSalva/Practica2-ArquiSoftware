using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages 
{
    public class MembershipsModel : PageModel
    {
        private readonly IRepository<Membership> _repo;

        public IEnumerable<Membership> MembershipList { get; private set; } = Enumerable.Empty<Membership>();

        public MembershipsModel(IRepository<Membership> repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            MembershipList = await _repo.GetAllAsync();
        }
    }
}