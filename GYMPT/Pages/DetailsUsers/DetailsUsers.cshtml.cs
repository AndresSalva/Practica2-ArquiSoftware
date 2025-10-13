using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.DetailsUsers
{
    public class DetailsUsersModel : PageModel
    {
        private readonly IDetailUserService _detailUserService;
        private readonly IUserService _userService;
        private readonly IMembershipService _membershipService;

        // --- Propiedades para la Lista ---
        public IEnumerable<DetailsUser> DetailUserList { get; set; } = new List<DetailsUser>();
        public Dictionary<int, string> UserNames { get; set; } = new Dictionary<int, string>();
        public Dictionary<short, string> MembershipNames { get; set; } = new Dictionary<short, string>();

        // --- Propiedades para el Formulario de Creaci�n ---
        [BindProperty]
        public DetailsUser NewDetailUser { get; set; } = new();
        public SelectList UserOptions { get; set; }
        public SelectList MembershipOptions { get; set; }

        public DetailsUsersModel(IDetailUserService detailUserService, IUserService userService, IMembershipService membershipService)
        {
            _detailUserService = detailUserService;
            _userService = userService;
            _membershipService = membershipService;
        }

        // Se ejecuta al cargar la p�gina
        public async Task OnGetAsync()
        {
            // Carga la lista principal
            DetailUserList = await _detailUserService.GetAllDetailUsers();

            // Carga los datos necesarios para los men�s desplegables y la tabla
            await PopulateRelatedData();
        }

        // Se ejecuta al enviar el formulario de CREACI�N
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Si hay un error, recarga la p�gina con la lista y los desplegables
                await OnGetAsync();
                return Page();
            }

            await _detailUserService.CreateNewDetailUser(NewDetailUser);
            TempData["SuccessMessage"] = "La suscripci�n del usuario ha sido registrada exitosamente.";
            return RedirectToPage();
        }

        // Se ejecuta al presionar el bot�n de ELIMINAR
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _detailUserService.DeleteDetailUser(id);
            TempData["SuccessMessage"] = "La suscripci�n ha sido eliminada.";
            return RedirectToPage();
        }

        // M�todo auxiliar para no repetir c�digo
        private async Task PopulateRelatedData()
        {
            var users = await _userService.GetAllUsers();
            var memberships = await _membershipService.GetAllMemberships();

            // Para la tabla (convertir IDs en Nombres)
            UserNames = users.ToDictionary(u => u.Id, u => $"{u.Name} {u.FirstLastname}");
            MembershipNames = memberships.ToDictionary(m => m.Id, m => m.Name);

            // Para los men�s desplegables del formulario de creaci�n
            UserOptions = new SelectList(users, "Id", "Name");
            MembershipOptions = new SelectList(memberships, "Id", "Name");
        }
    }
}