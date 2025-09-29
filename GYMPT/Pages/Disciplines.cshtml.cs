using GYMPT.Data;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages
{
    public class DisciplinesModel : PageModel
    {
        private readonly DisciplineRepository _repo;

        public IEnumerable<Discipline> DisciplineList { get; private set; } = Enumerable.Empty<Discipline>();

        public DisciplinesModel(DisciplineRepository repo)
        {
            _repo = repo;
        }
        public async Task OnGetAsync()
        {
            DisciplineList = await _repo.GetAllAsync();
        }
    }
}
