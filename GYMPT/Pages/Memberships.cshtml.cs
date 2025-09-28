using GYMPT.Data;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages 
{

    public class MembershipsModel : PageModel
    {
        private readonly IMembershipRepository _repo;

        public IEnumerable<Membership> MembershipList { get; private set; }

        public MembershipsModel(IMembershipRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            MembershipList = await _repo.GetAllAsync();
        }
    }
}