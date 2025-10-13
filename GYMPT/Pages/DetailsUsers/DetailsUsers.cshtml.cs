using GYMPT.Domain.Entities;               // Entidad del dominio (DetailsUser)
using GYMPT.Domain.Ports;                  // Interfaz genérica IRepository<T>
using GYMPT.Infrastructure.Factories;      // Fábrica del repositorio concreto
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.DetailsUsers
{
    public class DetailsUsersModel : PageModel
    {
        // -------------------------------
        // Propiedades del modelo
        // -------------------------------
        [BindProperty]
        public DetailsUser DetailUser { get; set; } = new DetailsUser();

        public List<DetailsUser> DetailsUserList { get; private set; } = new();

        // -------------------------------
        // Fábrica de repositorio
        // -------------------------------
        private IRepository<DetailsUser> CreateDetailUserRepository()
        {
            var factory = new DetailUserRepositoryCreator();
            return factory.CreateRepository();
        }

        // -------------------------------
        // GET: Cargar datos iniciales
        // -------------------------------
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                var repo = CreateDetailUserRepository();

                // Cargar registro individual si hay un ID
                if (id.HasValue)
                {
                    DetailUser = await repo.GetByIdAsync(id.Value);
                    if (DetailUser == null)
                        TempData["InfoMessage"] = "El detalle solicitado no fue encontrado.";
                }

                // Cargar todos los registros
                var details = await repo.GetAllAsync();
                DetailsUserList = details?.ToList() ?? new List<DetailsUser>();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los datos: {ex.Message}";
            }

            return Page();
        }

        // -------------------------------
        // POST: Crear nuevo detalle
        // -------------------------------
        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                    return Page();

                if (DetailUser.EndDate <= DetailUser.StartDate)
                {
                    TempData["ErrorMessage"] = "La fecha de fin debe ser posterior a la fecha de inicio.";
                    return RedirectToPage();
                }

                var repo = CreateDetailUserRepository();
                await repo.CreateAsync(DetailUser);

                TempData["SuccessMessage"] = "Detalle de usuario creado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el detalle: {ex.Message}";
            }

            return RedirectToPage();
        }

        // -------------------------------
        // POST: Actualizar detalle existente
        // -------------------------------
        public async Task<IActionResult> OnPostUpdateAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                    return Page();

                if (DetailUser.EndDate <= DetailUser.StartDate)
                {
                    TempData["ErrorMessage"] = "La fecha de fin debe ser posterior a la fecha de inicio.";
                    return RedirectToPage();
                }

                var repo = CreateDetailUserRepository();
                await repo.UpdateAsync(DetailUser);

                TempData["SuccessMessage"] = "Detalle de usuario actualizado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el detalle: {ex.Message}";
            }

            // Limpiar id tras la actualización para refrescar la vista
            return RedirectToPage(new { id = (int?)null });
        }

        // -------------------------------
        // POST: Eliminar detalle
        // -------------------------------
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var repo = CreateDetailUserRepository();
                bool deleted = await repo.DeleteByIdAsync(id);

                if (deleted)
                    TempData["SuccessMessage"] = "Detalle eliminado correctamente.";
                else
                    TempData["ErrorMessage"] = "No se pudo eliminar el detalle o no existe.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
