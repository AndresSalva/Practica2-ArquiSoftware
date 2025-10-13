using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Memberships
{
    public class MembershipEditModel : PageModel
    {
        private readonly IMembershipService _membershipService;

        [BindProperty]
        public Membership Membership { get; set; }

        public MembershipEditModel(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        // --- ESTE M�TODO CARGA LOS DATOS EN EL FORMULARIO ---
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Membership = await _membershipService.GetMembershipById(id);
            if (Membership == null)
            {
                TempData["ErrorMessage"] = "Membres�a no encontrada.";
                return RedirectToPage("/Memberships/Membership");
            }
            return Page();
        }

        // --- ESTE M�TODO GUARDA LOS CAMBIOS Y PONE EL MENSAJE DE �XITO ---
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var success = await _membershipService.UpdateMembershipData(Membership);
            if (success)
            {
                TempData["SuccessMessage"] = "Los datos de la membres�a han sido actualizados.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo actualizar la membres�a.";
            }
            return RedirectToPage("/Memberships/Membership");
        }
    }
}