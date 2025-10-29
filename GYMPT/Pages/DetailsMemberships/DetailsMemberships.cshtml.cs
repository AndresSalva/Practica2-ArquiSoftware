using System;
using System.Collections.Generic;
using System.Linq;
using GYMPT.Application.Interfaces;
using GYMPT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ServiceDiscipline.Application.Interfaces;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;

namespace GYMPT.Pages.DetailsMemberships;

[Authorize(Roles = "Admin")]
public class DetailsMembershipsModel : PageModel
{
    private readonly IDetailMembershipService _detailMembershipService;
    private readonly IMembershipService _membershipService;
    private readonly IDisciplineService _disciplineService;
    private readonly UrlTokenSingleton _urlTokenSingleton;

    public IEnumerable<DetailsMembership> DetailsMembershipList { get; private set; } = new List<DetailsMembership>();
    public Dictionary<short, string> MembershipNames { get; private set; } = new();
    public Dictionary<short, string> DisciplineNames { get; private set; } = new();
    public IReadOnlyList<MembershipDisciplineGroup> MembershipDisciplineGroups { get; private set; } = Array.Empty<MembershipDisciplineGroup>();

    public DetailsMembershipsModel(
        IDetailMembershipService detailMembershipService,
        IMembershipService membershipService,
        IDisciplineService disciplineService,
        UrlTokenSingleton urlTokenSingleton)
    {
        _detailMembershipService = detailMembershipService;
        _membershipService = membershipService;
        _disciplineService = disciplineService;
        _urlTokenSingleton = urlTokenSingleton;
    }

    public async Task OnGetAsync()
    {
        var detailsResult = await _detailMembershipService.GetAllDetailsMemberships();
        if (detailsResult.IsFailure || detailsResult.Value is null)
        {
            TempData["ErrorMessage"] = detailsResult.Error ?? "No se pudieron obtener los detalles de membresía.";
            DetailsMembershipList = Enumerable.Empty<DetailsMembership>();
        }
        else
        {
            DetailsMembershipList = detailsResult.Value;
        }

        await LoadReferenceDataAsync();
        BuildGroupedData();
    }

    public async Task<IActionResult> OnPostDeleteAsync(short membershipId, short disciplineId)
    {
        var result = await _detailMembershipService.DeleteDetailsMembership(membershipId, disciplineId);
        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess
            ? "La disciplina se eliminó de la membresía correctamente."
            : result.Error ?? "No se pudo eliminar la relación solicitada.";

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteMembershipAsync(short membershipId)
    {
        var result = await _detailMembershipService.DeleteDetailsForMembership(membershipId);
        TempData[result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = result.IsSuccess
            ? "Se eliminaron todas las disciplinas asociadas a la membresía."
            : result.Error ?? "No se pudieron eliminar las disciplinas de la membresía.";

        return RedirectToPage();
    }

    public string GetEncodedToken(short membershipId) => _urlTokenSingleton.GenerateToken(membershipId.ToString());

    private async Task LoadReferenceDataAsync()
    {
        var membershipResult = await _membershipService.GetAllMemberships();
        var disciplines = await _disciplineService.GetAllDisciplines();

        MembershipNames = membershipResult.IsSuccess && membershipResult.Value is not null
            ? membershipResult.Value.ToDictionary(m => m.Id, m => m.Name ?? $"Membresía #{m.Id}")
            : new Dictionary<short, string>();

        DisciplineNames = disciplines.ToDictionary(d => d.Id, d => d.Name ?? $"Disciplina #{d.Id}");
    }

    private void BuildGroupedData()
    {
        MembershipDisciplineGroups = DetailsMembershipList
            .GroupBy(detail => detail.IdMembership)
            .Select(detailGroup =>
            {
                var membershipId = detailGroup.Key;
                var membershipName = MembershipNames.TryGetValue(membershipId, out var name)
                    ? name
                    : $"Membresía #{membershipId}";

                var disciplines = detailGroup
                    .OrderBy(detail => DisciplineNames.TryGetValue(detail.IdDiscipline, out var dName) ? dName : detail.IdDiscipline.ToString())
                    .Select(detail => new DisciplineItem(
                        detail.IdDiscipline,
                        DisciplineNames.TryGetValue(detail.IdDiscipline, out var disciplineName)
                            ? disciplineName
                            : $"Disciplina #{detail.IdDiscipline}"
                    ))
                    .ToList();

                return new MembershipDisciplineGroup(membershipId, membershipName, disciplines);
            })
            .OrderBy(group => group.MembershipName)
            .ToList();
    }

    public sealed record MembershipDisciplineGroup(short MembershipId, string MembershipName, IReadOnlyList<DisciplineItem> Disciplines);
    public sealed record DisciplineItem(short DisciplineId, string DisciplineName);
}
