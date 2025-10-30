using GYMPT.Application.DTO;
using GYMPT.Application.Facades;
using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceCommon.Infrastructure.Services;

namespace GYMPT.Pages.Persons
{
    [Authorize]
    public class PersonModel : PageModel
    {
        private readonly PersonFacade _personFacade;
        private readonly ParameterProtector _urlTokenSingleton;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<PersonModel> _logger;

        public List<PersonDto> PersonList { get; set; } = new();
        public Dictionary<int, string> PersonTokens { get; set; } = new();

        public PersonModel(
            PersonFacade personFacade,
            ParameterProtector urlTokenSingleton,
            IUserContextService userContextService,
            ILogger<PersonModel> logger)
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

            var allPersons = await _personFacade.GetAllPersonsAsync();

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

            var duplicateIds = PersonList
                .GroupBy(p => p.Id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateIds.Any())
            {
                _logger.LogWarning("Se encontraron IDs duplicados en PersonList: {DuplicateIds}", string.Join(", ", duplicateIds));
            }

            PersonTokens = PersonList
                .GroupBy(p => p.Id)
                .Select(g => g.First())
                .ToDictionary(p => p.Id, p => _urlTokenSingleton.Protect(p.Id.ToString()));
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var allPersons = await _personFacade.GetAllPersonsAsync();
                var person = allPersons.FirstOrDefault(p => p.Id == id);

                if (person == null)
                {
                    TempData["ErrorMessage"] = "La persona ya no existe o no se encontró.";
                    return RedirectToPage();
                }

                bool success = false;

                if (person.Role == "Cliente" || person.Role == "Client")
                {
                    success = await _personFacade.DeleteClientAsync(id);
                }
                else
                {
                    success = await _personFacade.DeleteUserAsync(id);
                }

                TempData["SuccessMessage"] = success
                    ? $"Persona {person.Name} {person.FirstLastname} eliminada correctamente."
                    : "No se pudo eliminar la persona.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar persona con ID {Id}", id);
                TempData["ErrorMessage"] = "Ocurrió un error al eliminar la persona.";
            }

            return RedirectToPage();
        }

    }
}