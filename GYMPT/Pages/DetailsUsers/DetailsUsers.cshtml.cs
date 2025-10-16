using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GYMPT.Pages.DetailsUsers
{
    [Authorize]
    public class DetailsUsersModel : PageModel
    {
        // Servicios de negocio para las operaciones CRUD y de l�gica.
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
            // Carga la lista principal de detalles de usuario.
            DetailUserList = await _detailUserService.GetAllDetailUsers();

            // Carga todos los datos adicionales necesarios para la vista.
            await PopulateRelatedData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Si la validaci�n falla, debemos recargar los datos de los dropdowns
                // para que la p�gina se vuelva a mostrar correctamente.
                await PopulateRelatedData();
                return Page();
            }

            await _detailUserService.CreateNewDetailUser(NewDetailUser);
            TempData["SuccessMessage"] = "La suscripci�n del usuario ha sido registrada exitosamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _detailUserService.DeleteDetailUser(id);
            TempData["SuccessMessage"] = "La suscripci�n ha sido eliminada.";
            return RedirectToPage();
        }

        // Este m�todo ahora es mucho m�s declarativo y limpio.
        private async Task PopulateRelatedData()
        {
            // Pedimos los SelectLists ya preparados desde el servicio especializado.
            // La UI ya no sabe c�mo se construyen.
            UserOptions = await _selectDataService.GetUserOptionsAsync();
            MembershipOptions = await _selectDataService.GetMembershipOptionsAsync();

            var users = await _userService.GetAllUsers();
            var memberships = await _membershipService.GetAllMemberships();
            UserNames = users.ToDictionary(u => u.Id, u => $"{u.Name} {u.FirstLastname}");
            MembershipNames = memberships.ToDictionary(m => m.Id, m => m.Name);
        }
    }
}