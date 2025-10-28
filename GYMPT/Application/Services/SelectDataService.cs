// --- CAMBIO 1: Actualizar las directivas 'using' ---
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

// Se necesita el 'using' del nuevo módulo para IUserService
using ServiceClient.Application.Interfaces;

// AÚN necesitamos el 'using' antiguo para IMembershipService, que no se ha movido
using GYMPT.Application.Interfaces;

namespace GYMPT.Application.Services
{
    public class SelectDataService : ISelectDataService
    {
        // Este IUserService ahora apunta correctamente a ServiceClient.Application.Interfaces.IUserService
        private readonly IUserService _userService;
        // Este IMembershipService apunta correctamente a GYMPT.Application.Interfaces.IMembershipService
        private readonly IMembershipService _membershipService;

        public SelectDataService(IUserService userService, IMembershipService membershipService)
        {
            _userService = userService;
            _membershipService = membershipService;
        }

        public async Task<SelectList> GetUserOptionsAsync()
        {
            // --- CAMBIO 2: Usar el nombre de método correcto del nuevo contrato ---
            var users = await _userService.GetAllAsync(); // El método ahora se llama GetAllAsync

            var userOptions = users
                .Where(u => u.Role == "Client")
                .Select(u => new {
                    u.Id,
                    FullName = $"{u.Name} {u.FirstLastname}"
                });
            return new SelectList(userOptions, "Id", "FullName");
        }

        public async Task<SelectList> GetMembershipOptionsAsync()
        {
            // Esta parte no cambia porque IMembershipService no ha sido refactorizado
            var memberships = await _membershipService.GetAllMemberships();
            return new SelectList(memberships, "Id", "Name");
        }

        public async Task<SelectList> GetInstructorOptionsAsync()
        {
            // --- CAMBIO 2 (Repetido): Usar el nombre de método correcto del nuevo contrato ---
            var users = await _userService.GetAllAsync(); // El método ahora se llama GetAllAsync

            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new {
                    u.Id, // El Id en User es int, no long. No es necesario el cast.
                    FullName = $"{u.Name} {u.FirstLastname}"
                });
            return new SelectList(instructors, "Id", "FullName");
        }
    }
}