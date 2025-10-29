<<<<<<< HEAD
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using ServiceCommon.Infrastructure.Services;
// --- CAMBIO 1: Corregir las directivas 'using' ---
=======
﻿using GYMPT.Application.Facades;
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
>>>>>>> Service-Usuario
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
<<<<<<< HEAD
using System.Linq;
using System.Threading.Tasks;

// Se necesita el 'using' del nuevo módulo para IUserService.
using ServiceClient.Application.Interfaces;

// AÚN necesitamos los 'usings' antiguos para los servicios que no se han movido.
using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Infrastructure.Services;
=======
>>>>>>> Service-Usuario

namespace GYMPT.Pages.Disciplines
{
    [Authorize(Roles = "Admin")]
    public class DisciplineEditModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
<<<<<<< HEAD
        private readonly IUserService _userService;
        private readonly ParameterProtector _urlTokenSingleton;
=======
        private readonly SelectDataFacade _facade;
        private readonly UrlTokenSingleton _urlTokenSingleton;
>>>>>>> Service-Usuario

        [BindProperty]
        public Discipline Discipline { get; set; } = default!;
        public SelectList InstructorOptions { get; set; } = default!;

<<<<<<< HEAD
        public DisciplineEditModel(IDisciplineService disciplineService, IUserService userService, ParameterProtector urlTokenSingleton)
=======
        public DisciplineEditModel(
            IDisciplineService disciplineService,
            SelectDataFacade facade,
            UrlTokenSingleton urlTokenSingleton)
>>>>>>> Service-Usuario
        {
            _disciplineService = disciplineService;
            _facade = facade;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            var tokenId = _urlTokenSingleton.Unprotect(token);
            if (tokenId == null)
            {
                TempData["ErrorMessage"] = "Token inválido.";
                return RedirectToPage("./Discipline");
            }

<<<<<<< HEAD
            // --- CAMBIO 2: Estandarizar las llamadas a los métodos ---
            Discipline = await _disciplineService.GetByIdAsync(id); // El método correcto es GetByIdAsync
=======
            int id = int.Parse(tokenId);
            Discipline = await _disciplineService.GetDisciplineById(id);
>>>>>>> Service-Usuario
            if (Discipline == null)
            {
                TempData["ErrorMessage"] = "Disciplina no encontrada.";
                return RedirectToPage("./Discipline");
            }

            await PopulateInstructorsDropDownList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateInstructorsDropDownList();
                return Page();
            }

<<<<<<< HEAD
            // --- CAMBIO 2 (Continuación): Estandarizar la llamada al método ---
            var updatedDiscipline = await _disciplineService.UpdateAsync(Discipline); // El método correcto es UpdateAsync

            if (updatedDiscipline != null)
            {
                TempData["SuccessMessage"] = "Los datos de la disciplina han sido actualizados.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo actualizar la disciplina.";
            }
=======
            var success = await _disciplineService.UpdateDisciplineData(Discipline);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] =
                success ? "Los datos de la disciplina han sido actualizados."
                        : "No se pudo actualizar la disciplina.";
>>>>>>> Service-Usuario

            return RedirectToPage("./Discipline");
        }

        private async Task PopulateInstructorsDropDownList()
        {
<<<<<<< HEAD
            // --- CAMBIO 2 (Continuación): Estandarizar la llamada al método ---
            var users = await _userService.GetAllAsync(); // El método correcto es GetAllAsync
            var instructors = users
                .Where(u => u.Role == "Instructor")
                .Select(u => new { Id = (long)u.Id, FullName = $"{u.Name} {u.FirstLastname}" });

            InstructorOptions = new SelectList(instructors, "Id", "FullName", Discipline?.IdInstructor);
=======
            var instructors = await _facade.GetInstructorOptionsAsync();
            InstructorOptions = new SelectList(
                instructors,
                "Id",
                "FullName",
                Discipline?.IdInstructor
            );
>>>>>>> Service-Usuario
        }
    }
}
