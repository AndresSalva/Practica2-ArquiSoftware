using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Memberships
{
    [Authorize(Roles = "Admin")]
    public class MembershipEditModel : PageModel
    {
        private readonly IMembershipService _membershipService;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        [BindProperty]
        public Membership Membership { get; set; }

        public MembershipEditModel(IMembershipService membershipService, UrlTokenSingleton urlTokenSingleton)
        {
            _membershipService = membershipService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        // --- ESTE METODO CARGA LOS DATOS EN EL FORMULARIO ---
        public async Task<IActionResult> OnGetAsync(string token)
        {
            var tokenId = _urlTokenSingleton.GetTokenData(token);
            if (tokenId == null)
            {
                TempData["ErrorMessage"] = "Token invalido.";
                return RedirectToPage("/Memberships/Membership");
            }
            int id = int.Parse(tokenId);
            Membership = await _membershipService.GetMembershipById(id);
            if (Membership == null)
            {
                TempData["ErrorMessage"] = "Membresia no encontrada.";
                return RedirectToPage("/Memberships/Membership");
            }
            return Page();
        }

        // --- ESTE METODO GUARDA LOS CAMBIOS Y PONE EL MENSAJE DE EXITO ---
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