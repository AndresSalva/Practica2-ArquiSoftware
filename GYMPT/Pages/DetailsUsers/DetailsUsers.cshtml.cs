// --- CAMBIO 1: Limpiar y corregir las directivas 'using' ---
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Se necesita el 'using' del nuevo módulo para IUserService.
using ServiceClient.Application.Interfaces;

// AÚN necesitamos los 'usings' antiguos para los servicios que no se han movido.
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;

namespace GYMPT.Pages.DetailsUsers
{
    [Authorize]
    public class DetailsUsersModel : PageModel
    {
        private readonly IDetailUserService _detailUserService;
        private readonly IPersonService _userService;
        private readonly IMembershipService _membershipService;
        private readonly ISelectDataService _selectDataService;

        public IEnumerable<DetailsUser> DetailUserList { get; set; } = new List<DetailsUser>();
        public Dictionary<int, string> UserNames { get; set; } = new Dictionary<int, string>();
        public Dictionary<short, string> MembershipNames { get; set; } = new Dictionary<short, string>();

        [BindProperty]
        public DetailsUser NewDetailUser { get; set; } = new();
        public SelectList UserOptions { get; set; }
        public SelectList MembershipOptions { get; set; }

        public DetailsUsersModel(
            IDetailUserService detailUserService,
            IPersonService userService,
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
            // --- CAMBIO 2: Estandarizar las llamadas a los métodos ---
            DetailUserList = await _detailUserService.GetAllAsync(); // Asumiendo que el método estándar es GetAllAsync
            await PopulateRelatedData();
            NewDetailUser.StartDate = DateTime.Today;
            NewDetailUser.EndDate = DateTime.Today.AddDays(30);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateRelatedData();
                return Page();
            }

            await _detailUserService.CreateAsync(NewDetailUser); // Asumiendo que el método estándar es CreateAsync
            TempData["SuccessMessage"] = "La suscripcion del usuario ha sido registrada exitosamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _detailUserService.DeleteByIdAsync(id); // Asumiendo que el método estándar es DeleteByIdAsync
            TempData["SuccessMessage"] = "La suscripción ha sido eliminada.";
            return RedirectToPage();
        }

        private async Task PopulateRelatedData()
        {
            UserOptions = await _selectDataService.GetUserOptionsAsync();
            MembershipOptions = await _selectDataService.GetMembershipOptionsAsync();

            // --- ESTA ES LA LÍNEA DEL ERROR CORREGIDA ---
            var users = await _userService.GetAllAsync(); // El método correcto es GetAllAsync

            var memberships = await _membershipService.GetAllMemberships();
            UserNames = users.ToDictionary(u => u.Id, u => $"{u.Name} {u.FirstLastname}");
            MembershipNames = memberships.ToDictionary(m => m.Id, m => m.Name);
        }
    }
}