using System.Collections.Generic;
using System.Linq;
using GYMPT.Application.Interfaces;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Application.Common;

namespace GYMPT.Pages.DetailsMemberships;

[Authorize(Roles = "Admin")]
public class DetailsMembershipEditModel : PageModel
{
    private readonly IDetailMembershipService _detailMembershipService;
    private readonly ISelectDataService _selectDataService;
    private readonly IMembershipService _membershipService;
    private readonly UrlTokenSingleton _urlTokenSingleton;

    [BindProperty]
    public short MembershipId { get; set; }

    [BindProperty]
    public short SelectedMembershipId { get; set; }

    [BindProperty]
    public short OriginalMembershipId { get; set; }

    public string MembershipName { get; private set; } = string.Empty;

    [BindProperty]
    public List<short> SelectedDisciplineIds { get; set; } = new();

    public SelectList MembershipOptions { get; private set; } = default!;
    public SelectList DisciplineOptions { get; private set; } = default!;

    public DetailsMembershipEditModel(
        IDetailMembershipService detailMembershipService,
        ISelectDataService selectDataService,
        IMembershipService membershipService,
        UrlTokenSingleton urlTokenSingleton)
    {
        _detailMembershipService = detailMembershipService;
        _selectDataService = selectDataService;
        _membershipService = membershipService;
        _urlTokenSingleton = urlTokenSingleton;
    }

    public async Task<IActionResult> OnGetAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return NotFound();
        }

        var decoded = _urlTokenSingleton.GetTokenData(token);
        if (!short.TryParse(decoded, out var membershipId) || membershipId <= 0)
        {
            return NotFound();
        }

        OriginalMembershipId = membershipId;
        MembershipId = membershipId;
        SelectedMembershipId = membershipId;

        var loadResult = await LoadReferenceDataAsync(membershipId);
        if (!loadResult.IsSuccess)
        {
            TempData["ErrorMessage"] = loadResult.Error;
            return RedirectToPage("/DetailsMemberships/DetailsMemberships");
        }

        var currentDisciplines = await _detailMembershipService.GetDetailsMembershipsByMembership(membershipId);
        if (currentDisciplines.IsSuccess && currentDisciplines.Value is not null)
        {
            SelectedDisciplineIds = currentDisciplines.Value.Select(d => d.IdDiscipline).ToList();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        SelectedDisciplineIds ??= new List<short>();

        var loadResult = await LoadReferenceDataAsync(OriginalMembershipId);
        if (!loadResult.IsSuccess)
        {
            TempData["ErrorMessage"] = loadResult.Error;
            return RedirectToPage("/DetailsMemberships/DetailsMemberships");
        }

        if (SelectedMembershipId <= 0)
        {
            ModelState.AddModelError(nameof(SelectedMembershipId), "Selecciona una membresía válida.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (SelectedMembershipId == OriginalMembershipId)
        {
            var updateResult = await _detailMembershipService.UpdateDisciplinesForMembership(SelectedMembershipId, SelectedDisciplineIds);
            if (updateResult.IsFailure)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return Page();
            }
        }
        else
        {
            var assignResult = await _detailMembershipService.UpdateDisciplinesForMembership(SelectedMembershipId, SelectedDisciplineIds);
            if (assignResult.IsFailure)
            {
                foreach (var error in assignResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return Page();
            }

            var cleanResult = await _detailMembershipService.DeleteDetailsForMembership(OriginalMembershipId);
            if (cleanResult.IsFailure)
            {
                TempData["ErrorMessage"] = cleanResult.Error ?? "La membresía original conserva relaciones previas.";
            }

            OriginalMembershipId = SelectedMembershipId;
        }

        TempData["SuccessMessage"] = "Las disciplinas de la membresía se actualizaron correctamente.";
        return RedirectToPage("/DetailsMemberships/DetailsMemberships");
    }

    private async Task<Result> LoadReferenceDataAsync(short membershipId)
    {
        try
        {
            MembershipOptions = await _selectDataService.GetMembershipOptionsAsync();
        }
        catch (Exception ex)
        {
            return Result.Failure($"No se pudieron cargar las membresías: {ex.Message}");
        }

        DisciplineOptions = await _selectDataService.GetDisciplineOptionsAsync();

        var membershipResult = await _membershipService.GetMembershipById(SelectedMembershipId > 0 ? SelectedMembershipId : membershipId);
        if (membershipResult.IsFailure || membershipResult.Value is null)
        {
            return Result.Failure(membershipResult.Error ?? "No se encontró la membresía solicitada.");
        }

        MembershipName = membershipResult.Value.Name ?? $"Membresía #{membershipResult.Value.Id}";
        MembershipId = SelectedMembershipId > 0 ? SelectedMembershipId : membershipId;
        return Result.Success();
    }
}

