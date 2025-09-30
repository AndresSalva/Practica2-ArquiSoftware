using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages
{
    public class DisciplinesModel : PageModel
    {
        private readonly IRepository<Discipline> _repo;

        public IEnumerable<Discipline> DisciplineList { get; private set; } = Enumerable.Empty<Discipline>();

        public DisciplinesModel(IRepository<Discipline> repo)
        {
            _repo = repo;
        }
        public async Task OnGetAsync()
        {
            DisciplineList = await _repo.GetAllAsync();
        }
    }
}
