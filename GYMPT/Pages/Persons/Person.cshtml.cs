using GYMPT.Application.DTO;
using GYMPT.Application.Facades;
using GYMPT.Application.Interfaces;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace GYMPT.Pages.Persons
{
    [Authorize]
    public class PersonModel : PageModel
    {
        private readonly PersonFacade _personFacade;
        private readonly UrlTokenSingleton _urlTokenSingleton;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<PersonModel> _logger; // <-- Logger agregado

        public List<PersonDto> PersonList { get; set; } = new();
        public Dictionary<int, string> PersonTokens { get; set; } = new();

        public PersonModel(
            PersonFacade personFacade,
            UrlTokenSingleton urlTokenSingleton,
            IUserContextService userContextService,
            ILogger<PersonModel> logger)  // <-- Logger inyectado
        {
            _personFacade = personFacade;
            _urlTokenSingleton = urlTokenSingleton;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            var role = _userContextService.GetUserRole();
            var userId = _userContextService.GetUserId();

            // Obtener todas las personas
            var allPersons = await _personFacade.GetAllPersonsAsync();

            // Filtrar según rol
            PersonList = role switch
            {
                "Admin" => allPersons,
                "Instructor" when userId.HasValue => allPersons
                    .Where(p => p.Role == "Client" || (p.Role == "Instructor" && p.Id == userId.Value))
                    .ToList(),
                "Client" when userId.HasValue => allPersons
                    .Where(p => p.Id == userId.Value || p.Role == "Instructor")
                    .ToList(),
                _ => new List<PersonDto>()
            };

            // Detectar duplicados
            var duplicateIds = PersonList
                .GroupBy(p => p.Id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateIds.Any())
            {
                _logger.LogWarning("Se encontraron IDs duplicados en PersonList: {DuplicateIds}", string.Join(", ", duplicateIds));
            }

            // Generar tokens evitando duplicados
            PersonTokens = PersonList
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToDictionary(p => p.Id, p => _urlTokenSingleton.GenerateToken(p.Id.ToString()));
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var person = PersonList.FirstOrDefault(p => p.Id == id);
                if (person == null)
                {
                    TempData["ErrorMessage"] = "La persona ya no existe.";
                    return RedirectToPage();
                }

                bool success = person.Role == "Client"
                    ? await _personFacade.DeleteClientAsync(id)
                    : await _personFacade.DeleteUserAsync(id);

                TempData["SuccessMessage"] = success
                    ? $"Persona {person.Name} {person.FirstLastname} eliminada correctamente."
                    : "No se pudo eliminar la persona.";
            }
            catch
            {
                TempData["ErrorMessage"] = "Ocurrió un error al eliminar la persona.";
            }

            return RedirectToPage();
        }
    }
}