using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;

namespace GYMPT.Pages.Memberships
{
    [Authorize(Roles = "Admin")]
    public class MembershipCreateModel : PageModel
    {
        private readonly IMembershipService _membershipService;

        [BindProperty]
        public Membership Membership { get; set; } = new();

        public MembershipCreateModel(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            // Si la validaci�n falla, se queda en la p�gina y muestra los errores
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Llama al servicio para crear la membres�a
            await _membershipService.CreateNewMembership(Membership);

            // Prepara el mensaje de �xito para el pop-up
            TempData["SuccessMessage"] = $"La membres�a '{Membership.Name}' ha sido creada exitosamente.";

            // Redirige a la lista
            return RedirectToPage("/Memberships/Membership");
        }
    }
}
