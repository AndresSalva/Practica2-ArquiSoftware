using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
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
        private readonly IDetailUserService _detailUserService;

        [BindProperty]
        public DetailsUser DetailUser { get; set; } = new DetailsUser();

        public List<DetailsUser> DetailsUserList { get; set; } = new List<DetailsUser>();

        public DetailsUsersModel(IDetailUserService detailUserService)
        {
            _detailUserService = detailUserService;
        }

        public async Task OnGetAsync(int? id)
        {
            if (id.HasValue)
            {
                DetailUser = await _detailUserService.GetDetailUserById(id.Value) ?? new DetailsUser();
            }

            var details = await _detailUserService.GetAllDetailUsers();
            DetailsUserList = details?.ToList() ?? new List<DetailsUser>();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                // Es necesario recargar la lista si la validación falla para que la página se muestre bien
                await RehydratePageOnPostError();
                return Page();
            }

            if (DetailUser.EndDate <= DetailUser.StartDate)
            {
                ModelState.AddModelError("DetailUser.EndDate", "La fecha de fin debe ser posterior a la fecha de inicio.");
                await RehydratePageOnPostError();
                return Page();
            }

            await _detailUserService.CreateNewDetailUser(DetailUser);
            TempData["SuccessMessage"] = "Detalle de usuario creado exitosamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                await RehydratePageOnPostError();
                return Page();
            }

            if (DetailUser.EndDate <= DetailUser.StartDate)
            {
                ModelState.AddModelError("DetailUser.EndDate", "La fecha de fin debe ser posterior a la fecha de inicio.");
                await RehydratePageOnPostError();
                return Page();
            }

            await _detailUserService.UpdateDetailUserData(DetailUser);
            TempData["SuccessMessage"] = "Detalle de usuario actualizado exitosamente.";
            return RedirectToPage(new { id = (int?)null });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _detailUserService.DeleteDetailUser(id);
            TempData["SuccessMessage"] = "Detalle eliminado correctamente.";
            return RedirectToPage();
        }

        private async Task RehydratePageOnPostError()
        {
            var details = await _detailUserService.GetAllDetailUsers();
            DetailsUserList = details?.ToList() ?? new List<DetailsUser>();
        }
    }
}