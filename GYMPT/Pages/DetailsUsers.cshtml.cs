using GYMPT.Data.Repositories;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class DetailsUsersModel : PageModel
    {
        private readonly DetailUserRepository _detailUserRepository;

        public DetailsUsersModel(DetailUserRepository detailUserRepository)
        {
            _detailUserRepository = detailUserRepository;
        }

        [BindProperty]
        public DetailsUser DetailUser { get; set; } = new DetailsUser();

        public List<DetailsUser> DetailsUserList { get; set; } = new List<DetailsUser>();

        public async Task OnGetAsync()
        {
            try
            {
                var details = await _detailUserRepository.GetAllAsync();
                DetailsUserList = details?.ToList() ?? new List<DetailsUser>();

                if (!DetailsUserList.Any())
                {
                    TempData["InfoMessage"] = "No hay detalles de usuarios registrados.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los detalles: {ex.Message}";
                DetailsUserList = new List<DetailsUser>();
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    TempData["ErrorMessage"] = $"Errores de validaci�n: {errors}";
                    return RedirectToPage();
                }

                // Validaciones adicionales
                if (DetailUser.EndDate <= DetailUser.StartDate)
                {
                    TempData["ErrorMessage"] = "La fecha de fin debe ser posterior a la fecha de inicio.";
                    return RedirectToPage();
                }

                if (DetailUser.IdUser <= 0 || DetailUser.IdMembership <= 0)
                {
                    TempData["ErrorMessage"] = "Los IDs de usuario y membres�a deben ser mayores a 0.";
                    return RedirectToPage();
                }

                var createdDetail = await _detailUserRepository.CreateAsync(DetailUser);

                if (createdDetail != null && createdDetail.Id > 0)
                {
                    TempData["SuccessMessage"] = "Detalle de usuario creado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo crear el detalle de usuario.";
                }
            }
            catch (ArgumentException ex)
            {
                // Captura espec�ficamente errores de validaci�n de FK
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el detalle: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                    TempData["ErrorMessage"] = $"Errores de validaci�n: {errors}";
                    return RedirectToPage();
                }

                if (DetailUser.EndDate <= DetailUser.StartDate)
                {
                    TempData["ErrorMessage"] = "La fecha de fin debe ser posterior a la fecha de inicio.";
                    return RedirectToPage();
                }

                var existingDetail = await _detailUserRepository.GetByIdAsync(DetailUser.Id);
                if (existingDetail == null)
                {
                    TempData["ErrorMessage"] = "No se encontr� el detalle de usuario para actualizar.";
                    return RedirectToPage();
                }

                var updatedDetail = await _detailUserRepository.UpdateAsync(DetailUser);

                if (updatedDetail != null)
                {
                    TempData["SuccessMessage"] = "Detalle de usuario actualizado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo actualizar el detalle de usuario.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el detalle: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long id)
        {
            try
            {
                if (id <= 0)
                {
                    TempData["ErrorMessage"] = "ID inv�lido.";
                    return RedirectToPage();
                }

                var result = await _detailUserRepository.DeleteByIdAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Detalle eliminado correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el detalle o no existe.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}