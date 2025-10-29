using System;
using System.Collections.Generic;
using System.Linq;
using ServiceCommon.Application;
using ServiceMembership.Application.Interfaces;
using ServiceMembership.Domain.Entities;
using ServiceMembership.Domain.Ports;

namespace ServiceMembership.Application.Services;

public class DetailUserService : IDetailUserService
{
    private readonly IDetailUserRepository _detailUserRepository;

    public DetailUserService(IDetailUserRepository detailUserRepository)
    {
        _detailUserRepository = detailUserRepository;
    }

    public async Task<Result<DetailsUser>> GetDetailUserById(int id)
    {
        if (id <= 0)
        {
            return Result<DetailsUser>.Failure("El identificador del detalle de usuario debe ser mayor a cero.");
        }

        try
        {
            var detailUser = await _detailUserRepository.GetByIdAsync(id);
            return detailUser is null
                ? Result<DetailsUser>.Failure("No se encontró el detalle de usuario solicitado.")
                : Result<DetailsUser>.Success(detailUser);
        }
        catch (Exception ex)
        {
            return Result<DetailsUser>.Failure($"No se pudo obtener el detalle de usuario: {ex.Message}");
        }
    }

    public async Task<Result<IReadOnlyCollection<DetailsUser>>> GetAllDetailUsers()
    {
        try
        {
            var details = await _detailUserRepository.GetAllAsync();
            return Result<IReadOnlyCollection<DetailsUser>>.Success(details.ToList());
        }
        catch (Exception ex)
        {
            return Result<IReadOnlyCollection<DetailsUser>>.Failure($"No se pudo obtener la lista de detalles de usuario: {ex.Message}");
        }
    }

    public async Task<Result<DetailsUser>> CreateNewDetailUser(DetailsUser newDetailUser)
    {
        var validationErrors = ValidateDetailUser(newDetailUser, requireId: false);
        if (validationErrors.Any())
        {
            return Result<DetailsUser>.Failure(validationErrors);
        }

        try
        {
            var created = await _detailUserRepository.CreateAsync(newDetailUser);
            return Result<DetailsUser>.Success(created);
        }
        catch (Exception ex)
        {
            return Result<DetailsUser>.Failure($"No se pudo crear el detalle de usuario: {ex.Message}");
        }
    }

    public async Task<Result<DetailsUser>> UpdateDetailUserData(DetailsUser detailUserToUpdate)
    {
        var validationErrors = ValidateDetailUser(detailUserToUpdate, requireId: true);
        if (validationErrors.Any())
        {
            return Result<DetailsUser>.Failure(validationErrors);
        }

        try
        {
            var updated = await _detailUserRepository.UpdateAsync(detailUserToUpdate);
            return updated is null
                ? Result<DetailsUser>.Failure("No se encontró el detalle de usuario que deseas actualizar.")
                : Result<DetailsUser>.Success(updated);
        }
        catch (Exception ex)
        {
            return Result<DetailsUser>.Failure($"No se pudo actualizar el detalle de usuario: {ex.Message}");
        }
    }

    public async Task<Result> DeleteDetailUser(int id)
    {
        if (id <= 0)
        {
            return Result.Failure("El identificador del detalle de usuario debe ser mayor a cero.");
        }

        try
        {
            var deleted = await _detailUserRepository.DeleteByIdAsync(id);
            return deleted
                ? Result.Success()
                : Result.Failure("No se encontró el detalle de usuario que intentas eliminar.");
        }
        catch (Exception ex)
        {
            return Result.Failure($"No se pudo eliminar el detalle de usuario: {ex.Message}");
        }
    }

    private static IReadOnlyCollection<string> ValidateDetailUser(DetailsUser? detailUser, bool requireId)
    {
        var errors = new List<string>();
        if (detailUser is null)
        {
            errors.Add("El detalle de usuario es obligatorio.");
            return errors;
        }

        if (requireId && detailUser.Id <= 0)
        {
            errors.Add("El identificador del detalle de usuario debe ser mayor a cero.");
        }

        if (detailUser.IdUser is null || detailUser.IdUser <= 0)
        {
            errors.Add("Debes seleccionar un usuario válido.");
        }

        if (detailUser.IdMembership is null || detailUser.IdMembership <= 0)
        {
            errors.Add("Debes seleccionar una membresía válida.");
        }

        if (detailUser.StartDate == DateTime.MinValue)
        {
            errors.Add("La fecha de inicio es obligatoria.");
        }

        if (detailUser.EndDate == DateTime.MinValue)
        {
            errors.Add("La fecha de finalización es obligatoria.");
        }

        if (detailUser.StartDate > detailUser.EndDate)
        {
            errors.Add("La fecha de inicio no puede ser posterior a la fecha de finalización.");
        }

        if (detailUser.SessionsLeft is null)
        {
            errors.Add("Debes especificar la cantidad de sesiones restantes.");
        }
        else if (detailUser.SessionsLeft <= 0)
        {
            errors.Add("Las sesiones restantes deben ser mayores a cero.");
        }

        return errors;
    }
}

