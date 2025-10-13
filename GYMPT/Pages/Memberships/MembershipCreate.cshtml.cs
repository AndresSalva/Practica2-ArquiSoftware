using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Memberships
{
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
            // Si la validación falla, se queda en la página y muestra los errores
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Llama al servicio para crear la membresía
            await _membershipService.CreateNewMembership(Membership);

            // Prepara el mensaje de éxito para el pop-up
            TempData["SuccessMessage"] = $"La membresía '{Membership.Name}' ha sido creada exitosamente.";

            // Redirige a la lista
            return RedirectToPage("/Memberships/Membership");
        }
    }
}