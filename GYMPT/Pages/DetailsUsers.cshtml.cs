using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GYMPT.Pages
{
    public class DetailsUsersModel : PageModel
    {
        private readonly IRepository<DetailsUser> _detailUserRepository;

        public DetailsUsersModel(IRepository<DetailsUser> detailUserRepository)
        {
            _detailUserRepository = detailUserRepository;
        }

        public IEnumerable<DetailsUser> DetailsUserList { get; set; } = new List<DetailsUser>();

        [BindProperty]
        public DetailsUser DetailUser { get; set; } = new DetailsUser();

        public async Task OnGetAsync()
        {
            DetailsUserList = await _detailUserRepository.GetAllAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                DetailsUserList = await _detailUserRepository.GetAllAsync();
                return Page();
            }

            await _detailUserRepository.CreateAsync(DetailUser);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid || DetailUser.Id <= 0)
            {
                DetailsUserList = await _detailUserRepository.GetAllAsync();
                return Page();
            }

            await _detailUserRepository.UpdateAsync(DetailUser);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _detailUserRepository.DeleteByIdAsync(id);
            return RedirectToPage();
        }
    }
}
