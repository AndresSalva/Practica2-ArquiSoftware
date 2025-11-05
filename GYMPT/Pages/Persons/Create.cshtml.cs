using GYMPT.Application.DTO;
using GYMPT.Infrastructure.Facade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Persons
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserCreationFacade _userCreationFacade;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(UserCreationFacade userCreationFacade, ILogger<CreateModel> logger)
        {
            _userCreationFacade = userCreationFacade;
            _logger = logger;
        }

        [BindProperty]
        public UserInputModel Input { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _userCreationFacade.CreateUserAsync(Input);

            if (result.IsFailure)
            {
                _logger.LogWarning("Error al crear usuario: {Error}", result.Error);
                ModelState.AddModelError(string.Empty, result.Error);
                return Page();
            }

            TempData["SuccessMessage"] = $"El usuario '{result.Value.Name} {result.Value.FirstLastname}' ha sido creado exitosamente.";
            _logger.LogInformation("Usuario creado: {Name} {Lastname}", result.Value.Name, result.Value.FirstLastname);
            return RedirectToPage("/Persons/Person");
        }
    }
}