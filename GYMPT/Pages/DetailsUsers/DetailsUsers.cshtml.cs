using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;

namespace GYMPT.Pages.DetailsUsers
{
    [Authorize]
    public class DetailsUsersModel : PageModel
    {
        private readonly IDetailUserService _detailUserService;
        private readonly IUserService _userService;
        private readonly IMembershipService _membershipService;

        // Servicio especializado para preparar datos para la UI.
        private readonly ISelectDataService _selectDataService;

        // --- Propiedades para la Vista (.cshtml) ---
        public IEnumerable<DetailsUser> DetailUserList { get; set; } = new List<DetailsUser>();
        public Dictionary<int, string> UserNames { get; set; } = new Dictionary<int, string>();
        public Dictionary<short, string> MembershipNames { get; set; } = new Dictionary<short, string>();

        [BindProperty]
        public DetailsUser NewDetailUser { get; set; } = new();
        public SelectList UserOptions { get; set; }
        public SelectList MembershipOptions { get; set; }

        // Inyectamos todas las dependencias que la p�gina necesita, incluyendo el nuevo servicio.
        public DetailsUsersModel(
            IDetailUserService detailUserService,
            IUserService userService,
            IMembershipService membershipService,
            ISelectDataService selectDataService)
        {
            _detailUserService = detailUserService;
            _userService = userService;
            _membershipService = membershipService;
            _selectDataService = selectDataService;
        }

        public async Task OnGetAsync()
        {
            DetailUserList = await _detailUserService.GetAllDetailUsers();
            await PopulateRelatedData();
            NewDetailUser.StartDate = DateTime.Today;
            NewDetailUser.EndDate = DateTime.Today.AddDays(30);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // If fails, reload dropdown menu data
                await PopulateRelatedData();
                return Page();
            }

            await _detailUserService.CreateNewDetailUser(NewDetailUser);
            TempData["SuccessMessage"] = "La suscripcion del usuario ha sido registrada exitosamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _detailUserService.DeleteDetailUser(id);
            TempData["SuccessMessage"] = "La suscripci�n ha sido eliminada.";
            return RedirectToPage();
        }

        private async Task PopulateRelatedData()
        {
            UserOptions = await _selectDataService.GetUserOptionsAsync();
            MembershipOptions = await _selectDataService.GetMembershipOptionsAsync();

            var users = await _userService.GetAllUsers();
            var memberships = await _membershipService.GetAllMemberships();
            UserNames = users.ToDictionary(u => u.Id, u => $"{u.Name} {u.FirstLastname}");
            MembershipNames = memberships.ToDictionary(m => m.Id, m => m.Name);
        }
    }
}
