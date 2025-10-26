using System;
using System.Collections.Generic;
using System.Linq;
using ServiceMembership.Application.Common;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;
using ServiceMembership.Domain.Ports;

namespace ServiceMembership.Application.Services;

public class DetailMembershipService : IDetailMembershipService
{
    private readonly IDetailMembershipRepository _detailMembershipRepository;

    public DetailMembershipService(IDetailMembershipRepository detailMembershipRepository)
    {
        _detailMembershipRepository = detailMembershipRepository;
    }

    public async Task<Result<DetailsMembership>> CreateDetailsMembership(DetailsMembership newDetailsMembership)
    {
        var validationErrors = ValidateDetailsMembership(newDetailsMembership);
        if (validationErrors.Any())
        {
            return Result<DetailsMembership>.Failure(validationErrors);
        }

        try
        {
            var created = await _detailMembershipRepository.CreateAsync(newDetailsMembership);
            return Result<DetailsMembership>.Success(created);
        }
        catch (Exception ex)
        {
            return Result<DetailsMembership>.Failure($"No se pudo asociar la disciplina a la membresía: {ex.Message}");
        }
    }

    public async Task<Result> DeleteDetailsMembership(short membershipId, short disciplineId)
    {
        if (membershipId <= 0 || disciplineId <= 0)
        {
            return Result.Failure("Los identificadores de membresía y disciplina deben ser mayores a cero.");
        }

        try
        {
            var removed = await _detailMembershipRepository.DeleteByIdsAsync(membershipId, disciplineId);
            return removed
                ? Result.Success()
                : Result.Failure("No se encontró la relación de membresía y disciplina que intentas eliminar.");
        }
        catch (Exception ex)
        {
            return Result.Failure($"No se pudo eliminar la relación de membresía y disciplina: {ex.Message}");
        }
    }

    public async Task<Result> DeleteDetailsForMembership(short membershipId)
    {
        if (membershipId <= 0)
        {
            return Result.Failure("El identificador de la membresía debe ser mayor a cero.");
        }

        try
        {
            await _detailMembershipRepository.DeleteByMembershipIdAsync(membershipId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"No se pudieron eliminar las disciplinas de la membresía: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyCollection<DetailsMembership>>> GetAllDetailsMemberships()
    {
        try
        {
            var details = await _detailMembershipRepository.GetAllAsync();
            return Result<IReadOnlyCollection<DetailsMembership>>.Success(details.ToList());
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyCollection<DetailsMembership>>.Failure($"No se pudo obtener el listado de detalles de membresía: {ex.Message}");
        }
    }

    public async Task<Result<DetailsMembership>> GetDetailsMembership(short membershipId, short disciplineId)
    {
        if (membershipId <= 0 || disciplineId <= 0)
        {
            return Result<DetailsMembership>.Failure("Los identificadores de membresía y disciplina deben ser mayores a cero.");
        }

        try
        {
            var detail = await _detailMembershipRepository.GetByIdsAsync(membershipId, disciplineId);
            return detail is null
                ? Result<DetailsMembership>.Failure("No se encontró la relación solicitada de membresía y disciplina.")
                : Result<DetailsMembership>.Success(detail);
        }
        catch (Exception ex)
        {
            return Result<DetailsMembership>.Failure($"No se pudo obtener la relación solicitada: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyCollection<DetailsMembership>>> GetDetailsMembershipsByMembership(short membershipId)
    {
        if (membershipId <= 0)
        {
            return Result<IReadOnlyCollection<DetailsMembership>>.Failure("El identificador de la membresía debe ser mayor a cero.");
        }

        try
        {
            var details = await _detailMembershipRepository.GetByMembershipIdAsync(membershipId);
            return Result<IReadOnlyCollection<DetailsMembership>>.Success(details.ToList());
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyCollection<DetailsMembership>>.Failure($"No se pudieron obtener las disciplinas de la membresía: {ex.Message}");
        }
    }

    public async Task<Result<DetailsMembership>> UpdateDetailsMembership(short membershipId, short disciplineId, DetailsMembership updatedDetailsMembership)
    {
        var validationErrors = ValidateDetailsMembership(updatedDetailsMembership).ToList();
        if (membershipId <= 0 || disciplineId <= 0)
        {
            validationErrors.Add("Los identificadores actuales de membresía y disciplina deben ser mayores a cero.");
        }

        if (validationErrors.Any())
        {
            return Result<DetailsMembership>.Failure(validationErrors);
        }

        try
        {
            var updated = await _detailMembershipRepository.UpdateAsync(membershipId, disciplineId, updatedDetailsMembership);
            return updated is null
                ? Result<DetailsMembership>.Failure("No se encontró la relación de membresía y disciplina que deseas actualizar.")
                : Result<DetailsMembership>.Success(updated);
        }
        catch (Exception ex)
        {
            return Result<DetailsMembership>.Failure($"No se pudo actualizar la relación de membresía y disciplina: {ex.Message}");
        }
    }

    public async Task<Result> UpdateDisciplinesForMembership(short membershipId, IEnumerable<short> disciplineIds)
    {
        if (membershipId <= 0)
        {
            return Result.Failure("El identificador de la membresía debe ser mayor a cero.");
        }

        var desiredDisciplines = (disciplineIds ?? Array.Empty<short>())
            .Where(id => id > 0)
            .Distinct()
            .ToList();

        try
        {
            var currentRelations = await _detailMembershipRepository.GetByMembershipIdAsync(membershipId);
            var currentDisciplineIds = currentRelations.Select(r => r.IdDiscipline).ToList();

            var disciplinesToRemove = currentDisciplineIds.Except(desiredDisciplines).ToList();
            var disciplinesToAdd = desiredDisciplines.Except(currentDisciplineIds).ToList();

            var errors = new List<string>();

            foreach (var disciplineId in disciplinesToRemove)
            {
                var removed = await _detailMembershipRepository.DeleteByIdsAsync(membershipId, disciplineId);
                if (!removed)
                {
                    errors.Add($"No se pudo eliminar la disciplina {disciplineId} de la membresía.");
                }
            }

            foreach (var disciplineId in disciplinesToAdd)
            {
                var createResult = await CreateDetailsMembership(new DetailsMembership
                {
                    IdMembership = membershipId,
                    IdDiscipline = disciplineId
                });

                if (createResult.IsFailure)
                {
                    errors.AddRange(createResult.Errors);
                }
            }

            return errors.Count > 0 ? Result.Failure(errors) : Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"No se pudieron actualizar las disciplinas de la membresía: {ex.Message}");
        }
    }

    private static IReadOnlyCollection<string> ValidateDetailsMembership(DetailsMembership? detailsMembership)
    {
        var errors = new List<string>();
        if (detailsMembership is null)
        {
            errors.Add("El detalle de membresía es obligatorio.");
            return errors;
        }

        if (detailsMembership.IdMembership <= 0)
        {
            errors.Add("Debes seleccionar una membresía válida.");
        }

        if (detailsMembership.IdDiscipline <= 0)
        {
            errors.Add("Debes seleccionar una disciplina válida.");
        }

        return errors;
    }
}
