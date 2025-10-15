using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.DetailsUsers
{
    public class DetailsUsersModel : PageModel
    {
        private readonly IDetailUserService _detailUserService;
        private readonly IUserService _userService;
        private readonly IMembershipService _membershipService;

        public IEnumerable<DetailsUser> DetailUserList { get; set; } = new List<DetailsUser>();
        public Dictionary<int, string> UserNames { get; set; } = new Dictionary<int, string>();
        public Dictionary<short, string> MembershipNames { get; set; } = new Dictionary<short, string>();

        [BindProperty]
        public DetailsUser NewDetailUser { get; set; }

        public SelectList UserOptions { get; set; }
        public SelectList MembershipOptions { get; set; }

        public DetailsUsersModel(IDetailUserService detailUserService, IUserService userService, IMembershipService membershipService)
        {
            _detailUserService = detailUserService;
            _userService = userService;
            _membershipService = membershipService;

            // ===== CORRECCIÓN DE FECHAS =====
            // Inicializamos el objeto con valores por defecto para que el formulario no muestre "01/01/0001".
            NewDetailUser = new DetailsUser
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(1) // Por ejemplo, un mes por defecto
            };
        }

        public async Task OnGetAsync()
        {
            DetailUserList = await _detailUserService.GetAllDetailUsers();
            await PopulateRelatedData();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }
            await _detailUserService.CreateNewDetailUser(NewDetailUser);
            TempData["SuccessMessage"] = "La suscripción del usuario ha sido registrada exitosamente.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _detailUserService.DeleteDetailUser(id);
            TempData["SuccessMessage"] = "La suscripción ha sido eliminada.";
            return RedirectToPage();
        }

        private async Task PopulateRelatedData()
        {
            var users = await _userService.GetAllUsers();
            var memberships = await _membershipService.GetAllMemberships();
            UserNames = users.ToDictionary(u => u.Id, u => $"{u.Name} {u.FirstLastname}");
            MembershipNames = memberships.ToDictionary(m => m.Id, m => m.Name);
            UserOptions = new SelectList(users.OrderBy(u => u.Name), "Id", "Name");
            MembershipOptions = new SelectList(memberships.OrderBy(m => m.Name), "Id", "Name");
        }
    }
}