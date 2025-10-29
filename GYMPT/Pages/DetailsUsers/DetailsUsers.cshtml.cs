<<<<<<< HEAD
// --- CAMBIO 1: Limpiar y corregir las directivas 'using' ---
=======
﻿using System;
using System.Collections.Generic;
using System.Linq;
using GYMPT.Application.Interfaces;
>>>>>>> Modulo_Membership
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
<<<<<<< HEAD
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Se necesita el 'using' del nuevo módulo para IUserService.
using ServiceClient.Application.Interfaces;

// AÚN necesitamos los 'usings' antiguos para los servicios que no se han movido.
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
=======
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;
>>>>>>> Modulo_Membership

namespace GYMPT.Pages.DetailsUsers
{
    [Authorize]
    public class DetailsUsersModel : PageModel
    {
        private readonly IDetailUserService _detailUserService;
        private readonly IPersonService _userService;
        private readonly IMembershipService _membershipService;
        private readonly ISelectDataService _selectDataService;

        public IEnumerable<DetailsUser> DetailUserList { get; set; } = new List<DetailsUser>();
        public Dictionary<int, string> UserNames { get; set; } = new();
        public Dictionary<short, string> MembershipNames { get; set; } = new();

        [BindProperty]
        public DetailsUser NewDetailUser { get; set; } = new();

<<<<<<< HEAD
=======
        [BindProperty] 
        public DetailsUser DetailUserToUpdate { get; set; } = new();
        public SelectList UserOptions { get; set; } = default!;
        public SelectList MembershipOptions { get; set; } = default!;

>>>>>>> Modulo_Membership
        public DetailsUsersModel(
            IDetailUserService detailUserService,
            IPersonService userService,
            IMembershipService membershipService,
            ISelectDataService selectDataService)
        {
            _detailUserService = detailUserService;
            _userService = userService;
            _membershipService = membershipService;
            _selectDataService = selectDataService;
        }

        public async Task OnGetAsync()
        {
<<<<<<< HEAD
            // --- CAMBIO 2: Estandarizar las llamadas a los métodos ---
            DetailUserList = await _detailUserService.GetAllAsync(); // Asumiendo que el método estándar es GetAllAsync
=======
            var detailsResult = await _detailUserService.GetAllDetailUsers();
            if (detailsResult.IsFailure || detailsResult.Value is null)
            {
                TempData["ErrorMessage"] = detailsResult.Error ?? "No se pudo obtener el listado de suscripciones.";
                DetailUserList = Enumerable.Empty<DetailsUser>();
            }
            else
            {
                DetailUserList = detailsResult.Value;
            }

>>>>>>> Modulo_Membership
            await PopulateRelatedData();
            NewDetailUser.StartDate = DateTime.Today;
            NewDetailUser.EndDate = DateTime.Today.AddDays(30);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateRelatedData();
                return Page();
            }

<<<<<<< HEAD
            await _detailUserService.CreateAsync(NewDetailUser); // Asumiendo que el método estándar es CreateAsync
            TempData["SuccessMessage"] = "La suscripcion del usuario ha sido registrada exitosamente.";
=======
            var result = await _detailUserService.CreateNewDetailUser(NewDetailUser);
            if (result.IsFailure)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                await PopulateRelatedData();
                return Page();
            }

            TempData["SuccessMessage"] = "La suscripción del usuario ha sido registrada exitosamente.";
>>>>>>> Modulo_Membership
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
<<<<<<< HEAD
            await _detailUserService.DeleteByIdAsync(id); // Asumiendo que el método estándar es DeleteByIdAsync
            TempData["SuccessMessage"] = "La suscripción ha sido eliminada.";
=======
            var result = await _detailUserService.DeleteDetailUser(id);
            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess
                ? "La suscripción ha sido eliminada."
                : result.Error ?? "No se pudo eliminar la suscripción.";
>>>>>>> Modulo_Membership
            return RedirectToPage();
        }

        private async Task PopulateRelatedData()
        {
            try
            {
                UserOptions = await _selectDataService.GetUserOptionsAsync();
                MembershipOptions = await _selectDataService.GetMembershipOptionsAsync();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                UserOptions = new SelectList(Enumerable.Empty<SelectListItem>());
                MembershipOptions = new SelectList(Enumerable.Empty<SelectListItem>());
            }

<<<<<<< HEAD
            // --- ESTA ES LA LÍNEA DEL ERROR CORREGIDA ---
            var users = await _userService.GetAllAsync(); // El método correcto es GetAllAsync

            var memberships = await _membershipService.GetAllMemberships();
=======
            var users = await _userService.GetAllUsers();
            var membershipsResult = await _membershipService.GetAllMemberships();

>>>>>>> Modulo_Membership
            UserNames = users.ToDictionary(u => u.Id, u => $"{u.Name} {u.FirstLastname}");
            MembershipNames = membershipsResult.IsSuccess && membershipsResult.Value is not null
                ? membershipsResult.Value.ToDictionary(m => m.Id, m => m.Name ?? $"Membresía #{m.Id}")
                : new Dictionary<short, string>();
        }
        public async Task<IActionResult> OnGetDetailUserAsync(int id)
        {
            
            var result = await _detailUserService.GetDetailUserById(id);
            if (result.IsSuccess && result.Value is not null)
            {
                return new JsonResult(result.Value);
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            
            if (!ModelState.IsValid)
            {
                // Si hay un error de validación, es difícil mostrarlo en el modal sin
                // una lógica de cliente más compleja. Por ahora, redirigimos con un error genérico.
                TempData["ErrorMessage"] = "Los datos proporcionados no son válidos. Por favor, inténtelo de nuevo.";
                return RedirectToPage();
            }

            var result = await _detailUserService.UpdateDetailUserData(DetailUserToUpdate);

            TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess
                ? "La suscripción ha sido actualizada exitosamente."
                : result.Error ?? "No se pudo actualizar la suscripción.";

            return RedirectToPage();
        }
    }
}
