using System;
using System.Linq;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;

namespace GYMPT.Pages.Memberships
{
    [Authorize(Roles = "Admin")]
    public class MembershipEditModel : PageModel
    {
        private readonly IMembershipService _membershipService;
        private readonly ParameterProtector _urlTokenSingleton;

        [BindProperty]
        public Membership Membership { get; set; } = new();

        public MembershipEditModel(IMembershipService membershipService, ParameterProtector urlTokenSingleton)
        {
            _membershipService = membershipService;
            _urlTokenSingleton = urlTokenSingleton;
        }

        public async Task<IActionResult> OnGetAsync(string token)
        {
            var tokenData = _urlTokenSingleton.GetTokenData(token);
            if (tokenData is null)
            {
                TempData["ErrorMessage"] = "Token inválido.";
                return RedirectToPage("/Memberships/Membership");
            }

            if (!int.TryParse(tokenData, out var membershipId))
            {
                TempData["ErrorMessage"] = "Identificador de membresía inválido.";
                return RedirectToPage("/Memberships/Membership");
            }

            var result = await _membershipService.GetMembershipById(membershipId);
            if (result.IsFailure || result.Value is null)
            {
                TempData["ErrorMessage"] = result.Error ?? "No se encontró la membresía solicitada.";
                return RedirectToPage("/Memberships/Membership");
            }

            Membership = result.Value;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _membershipService.UpdateMembershipData(Membership);
            if (result.IsFailure)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return Page();
            }

            TempData["SuccessMessage"] = "Los datos de la membresía han sido actualizados.";
            return RedirectToPage("/Memberships/Membership");
        }
    }
}
