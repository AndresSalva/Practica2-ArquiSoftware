using ServiceUser.Application.Common;
using ServiceUser.Application.Interfaces;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;
using ServiceUser.Domain.Rules; 

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<User>> CreateUser(User newUser)
    {
        var validationResult = UserValidator.Validar(newUser);
        if (validationResult.IsFailure)
        {
            return validationResult; 
        }

        var createdUser = await _userRepository.CreateAsync(newUser);
        return Result<User>.Success(createdUser);
    }

    public async Task<Result<User>> UpdateUser(User userToUpdate)
    {
        var validationResult = UserValidator.Validar(userToUpdate);
        if (validationResult.IsFailure)
        {
            return validationResult;
        }

        var updatedUser = await _userRepository.UpdateAsync(userToUpdate);
        if (updatedUser == null)
        {
            return Result<User>.Failure($"No se encontró el usuario con ID {userToUpdate.Id} para actualizar.");
        }

        return Result<User>.Success(updatedUser);
    }

    public async Task<Result<User>> GetUserById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return Result<User>.Failure($"No se encontró el usuario con ID {id}.");
        }
        return Result<User>.Success(user);
    }

    public async Task<Result<bool>> DeleteUser(int userId)
    {
        var success = await _userRepository.DeleteByIdAsync(userId);
        if (!success)
        {
            return Result<bool>.Failure($"No se pudo eliminar el usuario con ID {userId} (probablemente no se encontró).");
        }
        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UpdatePassword(int userId, string newPassword)
    {
        var passwordValidationResult = PasswordRules.Validar(newPassword);
        if (passwordValidationResult.IsFailure)
        {
            return Result<bool>.Failure(passwordValidationResult.Error);
        }

        var success = await _userRepository.UpdatePasswordAsync(userId, newPassword);
        if (!success)
        {
            return Result<bool>.Failure($"No se pudo actualizar la contraseña para el usuario con ID {userId}.");
        }
        return Result<bool>.Success(true);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userRepository.GetAllAsync();
    }
}