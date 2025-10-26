using System;
using System.Collections.Generic;
using System.Linq;
using ServiceMembership.Application.Common;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;
using ServiceMembership.Domain.Ports;

namespace ServiceMembership.Application.Services;

public class MembershipService : IMembershipService
{
    private readonly IMembershipRepository _membershipRepository;

    public MembershipService(IMembershipRepository membershipRepository)
    {
        _membershipRepository = membershipRepository;
    }

    public async Task<Result<Membership>> GetMembershipById(int id)
    {
        if (id <= 0)
        {
            return Result<Membership>.Failure("El identificador de la membresía debe ser mayor a cero.");
        }

        try
        {
            var membership = await _membershipRepository.GetByIdAsync(id);
            return membership is null
                ? Result<Membership>.Failure("No se encontró la membresía solicitada.")
                : Result<Membership>.Success(membership);
        }
        catch (Exception ex)
        {
            return Result<Membership>.Failure($"No se pudo obtener la membresía: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyCollection<Membership>>> GetAllMemberships()
    {
        try
        {
            var memberships = await _membershipRepository.GetAllAsync();
            return Result<IReadOnlyCollection<Membership>>.Success(memberships.ToList());
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyCollection<Membership>>.Failure($"No se pudo obtener el listado de membresías: {ex.Message}");
        }
    }

    public async Task<Result<Membership>> CreateNewMembership(Membership newMembership)
    {
        var validationErrors = ValidateMembership(newMembership, requireId: false);
        if (validationErrors.Any())
        {
            return Result<Membership>.Failure(validationErrors);
        }

        try
        {
            var createdMembership = await _membershipRepository.CreateAsync(newMembership);
            return Result<Membership>.Success(createdMembership);
        }
        catch (Exception ex)
        {
            return Result<Membership>.Failure($"No se pudo crear la membresía: {ex.Message}");
        }
    }

    public async Task<Result<Membership>> UpdateMembershipData(Membership membershipToUpdate)
    {
        var validationErrors = ValidateMembership(membershipToUpdate, requireId: true);
        if (validationErrors.Any())
        {
            return Result<Membership>.Failure(validationErrors);
        }

        try
        {
            var updatedMembership = await _membershipRepository.UpdateAsync(membershipToUpdate);
            return updatedMembership is null
                ? Result<Membership>.Failure("No se encontró la membresía que intentas actualizar.")
                : Result<Membership>.Success(updatedMembership);
        }
        catch (Exception ex)
        {
            return Result<Membership>.Failure($"No se pudo actualizar la membresía: {ex.Message}");
        }
    }

    public async Task<Result> DeleteMembership(int id)
    {
        if (id <= 0)
        {
            return Result.Failure("El identificador de la membresía debe ser mayor a cero.");
        }

        try
        {
            var deleted = await _membershipRepository.DeleteByIdAsync(id);
            return deleted
                ? Result.Success()
                : Result.Failure("No se encontró la membresía que se intenta eliminar.");
        }
        catch (Exception ex)
        {
            return Result.Failure($"No se pudo eliminar la membresía: {ex.Message}");
        }
    }

    private static IReadOnlyCollection<string> ValidateMembership(Membership? membership, bool requireId)
    {
        var errors = new List<string>();
        if (membership is null)
        {
            errors.Add("La membresía es obligatoria.");
            return errors;
        }

        if (requireId && membership.Id <= 0)
        {
            errors.Add("El identificador de la membresía debe ser mayor a cero.");
        }

        if (string.IsNullOrWhiteSpace(membership.Name))
        {
            errors.Add("El nombre de la membresía es obligatorio.");
        }
        else if (membership.Name.Length > 50)
        {
            errors.Add("El nombre de la membresía no puede exceder los 50 caracteres.");
        }

        if (membership.Price is < 0)
        {
            errors.Add("El precio de la membresía no puede ser negativo.");
        }

        if (membership.MonthlySessions is < 0)
        {
            errors.Add("Las sesiones mensuales no pueden ser negativas.");
        }

        return errors;
    }
}
