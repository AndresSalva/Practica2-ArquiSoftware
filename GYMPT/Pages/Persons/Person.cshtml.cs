using GYMPT.Application.DTO;
using GYMPT.Application.Facades;
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceUser.Domain.Entities;

namespace GYMPT.Pages.Persons
{
    [Authorize]
    public class PersonModel : PageModel
    {
        private readonly PersonFacade _personFacade;
        private readonly UrlTokenSingleton _urlTokenSingleton;

        public List<PersonDto> PersonList { get; set; } = new();
        public Dictionary<int, string> PersonTokens { get; set; } = new();

        public PersonModel(PersonFacade personFacade, UrlTokenSingleton urlTokenSingleton)
        {
            _personFacade = personFacade;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task OnGetAsync()
        {
            // Obtener todos los usuarios + clientes mapeados a PersonDto
            PersonList = await _personFacade.GetAllPersonsAsync();

            // Generar tokens por cada persona
            PersonTokens = PersonList.ToDictionary(p => p.Id, p => _urlTokenSingleton.GenerateToken(p.Id.ToString()));
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                // Aquí dependerá de qué tipo sea la persona
                var person = PersonList.FirstOrDefault(p => p.Id == id);
                if (person == null)
                {
                    TempData["ErrorMessage"] = "La persona que intentas eliminar ya no existe.";
                    return RedirectToPage();
                }

                bool success = false;

                // Llamar al servicio correspondiente según rol
                if (person.Role == "Client")
                {
                    success = await _personFacade.DeleteClientAsync(id);
                }
                else
                {
                    success = await _personFacade.DeleteUserAsync(id);
                }

                if (success)
                {
                    TempData["SuccessMessage"] = $"La persona {person.Name} {person.FirstLastname} fue eliminada correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar la persona. Es posible que ya haya sido eliminada.";
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al intentar eliminar la persona.";
            }

            return RedirectToPage();
        }
    }
}