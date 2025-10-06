using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace GYMPT.Pages.Users
{
    public class EditUserModel : PageModel
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<EditUserModel> _logger;

        public EditUserModel(IUserRepository userRepo, ILogger<EditUserModel> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        [BindProperty]
        public UserData User { get; set; }

        [BindProperty(SupportsGet = true)]
        public long Id { get; set; }

        public bool IsClient => User?.Role == "Client";
        public bool IsInstructor => User?.Role == "Instructor";

        public async Task<IActionResult> OnGetAsync()
        {
            _logger.LogInformation("➡️ Entrando a OnGetAsync con Id={Id}", Id);

            if (Id == 0)
            {
                _logger.LogWarning("Id = 0, redirigiendo a /Users");
                return RedirectToPage("/Users");
            }

            User = await _userRepo.GetByIdAsync(Id);

            if (User == null)
            {
                _logger.LogWarning("No se encontró usuario con Id={Id}", Id);
                return RedirectToPage("/Users");
            }

            _logger.LogInformation("Usuario encontrado: {Name}, Role={Role}", User.Name, User.Role);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("➡️ Entrando a OnPostAsync con Id={Id}", Id);

            if (User == null || Id == 0)
            {
                _logger.LogWarning("Usuario o Id inválido, redirigiendo a /Users");
                return RedirectToPage("/Users");
            }

            bool updated = await _userRepo.UpdateAsync(User);

            if (!updated)
            {
                _logger.LogError("No se pudo actualizar el usuario {Name}", User.Name);
                TempData["Error"] = "No se pudo actualizar el usuario.";
                return Page();
            }

            _logger.LogInformation("Usuario {Name} actualizado correctamente", User.Name);

            if (User.Role == "Client")
                return RedirectToPage("/Clients/EditClient", new { id = User.Id });
            else if (User.Role == "Instructor")
                return RedirectToPage("/Instructors/EditInstructor", new { id = User.Id });

            return RedirectToPage("/Users");
        }

    }
}
