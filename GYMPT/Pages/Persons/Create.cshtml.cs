using GYMPT.Application.DTO;
using GYMPT.Application.Facade;
using GYMPT.Application.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages.Persons
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserCreationFacade _userCreationFacade;
        private readonly ClientCreationFacade _clientCreationFacade;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(UserCreationFacade userCreationFacade, ClientCreationFacade clientCreationFacade,ILogger<CreateModel> logger)
        {
            _userCreationFacade = userCreationFacade;
            _clientCreationFacade = clientCreationFacade;
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

            if (Input.Role == "Instructor")
            {
                var resultUser = await _userCreationFacade.CreateUserAsync(Input);
                if (!resultUser.IsSuccess)
                {
                    _logger.LogWarning("Error al crear instructor: {Error}", resultUser.Error);
                    ModelState.AddModelError(string.Empty, resultUser.Error);
                    return Page();
                }

                TempData["SuccessMessage"] = $"El usuario '{resultUser.Value.Name} {resultUser.Value.FirstLastname}' ha sido creado exitosamente.";
                _logger.LogInformation("Instructor creado: {Name} {Lastname}", resultUser.Value.Name, resultUser.Value.FirstLastname);
            }
            else if (Input.Role == "Client")
            {
                var resultClient = await _clientCreationFacade.CreateClientAsync(Input);
                if (!resultClient.IsSuccess)
                {
                    _logger.LogWarning("Error al crear cliente: {Error}", resultClient.Error);
                    ModelState.AddModelError(string.Empty, resultClient.Error);
                    return Page();
                }

                TempData["SuccessMessage"] = $"El cliente '{resultClient.Value.Name} {resultClient.Value.FirstLastname}' ha sido creado exitosamente.";
                _logger.LogInformation("Cliente creado: {Name} {Lastname}", resultClient.Value.Name, resultClient.Value.FirstLastname);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Rol de usuario no válido.");
                return Page();
            }

            return RedirectToPage("/Persons/Person");
        }
    }
}