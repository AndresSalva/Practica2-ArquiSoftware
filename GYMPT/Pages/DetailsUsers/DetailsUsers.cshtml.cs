using GYMPT.Data.Contracts;
using GYMPT.Factories;
using GYMPT.Models;
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
        [BindProperty]
        public DetailsUser DetailUser { get; set; } = new DetailsUser();

        public List<DetailsUser> DetailsUserList { get; set; } = new List<DetailsUser>();

        public DetailsUsersModel() { }


        private IRepository<DetailsUser> CreateDetailUserRepository()
        {
            RepositoryCreator<DetailsUser> factory = new DetailUserRepositoryCreator();
            return factory.CreateRepository();
        }

  
        public async Task OnGetAsync(int? id)
        {
            var detailRepo = CreateDetailUserRepository();
            try
            {
                if (id.HasValue)
                {
                    DetailUser = await detailRepo.GetByIdAsync(id.Value);
                }

                var details = await detailRepo.GetAllAsync();
                DetailsUserList = details?.ToList() ?? new List<DetailsUser>();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al cargar los datos: {ex.Message}";
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            try
            {
                if (!ModelState.IsValid) return Page();
                if (DetailUser.EndDate <= DetailUser.StartDate)
                {
                    TempData["ErrorMessage"] = "La fecha de fin debe ser posterior a la fecha de inicio.";
                    return RedirectToPage();
                }

                var detailRepo = CreateDetailUserRepository();
                await detailRepo.CreateAsync(DetailUser);
                TempData["SuccessMessage"] = "Detalle de usuario creado exitosamente.";
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
                if (!ModelState.IsValid) return Page();
                if (DetailUser.EndDate <= DetailUser.StartDate)
                {
                    TempData["ErrorMessage"] = "La fecha de fin debe ser posterior a la fecha de inicio.";
                    return RedirectToPage();
                }

                var detailRepo = CreateDetailUserRepository();
                await detailRepo.UpdateAsync(DetailUser);
                TempData["SuccessMessage"] = "Detalle de usuario actualizado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el detalle: {ex.Message}";
            }
            return RedirectToPage(new { id = (int?)null }); 
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var detailRepo = CreateDetailUserRepository();
                var result = await detailRepo.DeleteByIdAsync(id);
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