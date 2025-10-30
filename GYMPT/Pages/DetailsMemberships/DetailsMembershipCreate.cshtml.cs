using GYMPT.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;

namespace GYMPT.Pages.DetailsMemberships;

[Authorize(Roles = "Admin")]
public class DetailsMembershipCreateModel : PageModel
{
    private readonly IDetailMembershipService _detailMembershipService;
    private readonly ISelectDataService _selectDataService;

    [BindProperty]
    public short SelectedMembershipId { get; set; }

    [BindProperty]
    public List<short> SelectedDisciplineIds { get; set; } = new();

    public SelectList MembershipOptions { get; private set; } = default!;
    public SelectList DisciplineOptions { get; private set; } = default!;

    public DetailsMembershipCreateModel(
        IDetailMembershipService detailMembershipService,
        ISelectDataService selectDataService)
    {
        _detailMembershipService = detailMembershipService;
        _selectDataService = selectDataService;
    }

    public async Task OnGetAsync()
    {
        await LoadOptionsAsync();
        SelectedDisciplineIds = new();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await LoadOptionsAsync();

        const string membershipWord = "membresía";

        if (SelectedMembershipId <= 0)
        {
            ModelState.AddModelError(nameof(SelectedMembershipId), $"Selecciona una {membershipWord} válida.");
        }

        if (SelectedDisciplineIds is null || SelectedDisciplineIds.Count == 0)
        {
            ModelState.AddModelError(nameof(SelectedDisciplineIds), "Selecciona al menos una disciplina.");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        SelectedDisciplineIds ??= new List<short>();

        var successCount = 0;
        var failureMessages = new List<string>();

        foreach (var disciplineId in SelectedDisciplineIds.Distinct())
        {
            var detail = new DetailsMembership
            {
                IdMembership = SelectedMembershipId,
                IdDiscipline = disciplineId
            };

            var result = await _detailMembershipService.CreateDetailsMembership(detail);
            if (result.IsSuccess)
            {
                successCount++;
            }
            else
            {
                failureMessages.AddRange(result.Errors);
            }
        }

        if (successCount > 0)
        {
            var successMessage = successCount == 1
                ? $"La disciplina fue asociada a la {membershipWord} correctamente."
                : $"Se asociaron {successCount} disciplinas a la {membershipWord}.";
            TempData["SuccessMessage"] = successMessage;
        }

        if (failureMessages.Count > 0)
        {
            TempData["ErrorMessage"] = string.Join(" ", failureMessages.Distinct());
        }

        if (successCount > 0)
        {
            return RedirectToPage("/DetailsMemberships/DetailsMemberships");
        }

        ModelState.AddModelError(string.Empty, failureMessages.FirstOrDefault() ?? "No se pudo registrar la asociación solicitada.");
        return Page();
    }

    private async Task LoadOptionsAsync()
    {
        MembershipOptions = await _selectDataService.GetMembershipOptionsAsync();
        DisciplineOptions = await _selectDataService.GetDisciplineOptionsAsync();
    }
}
