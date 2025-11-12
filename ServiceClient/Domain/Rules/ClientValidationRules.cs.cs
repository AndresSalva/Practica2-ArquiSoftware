// Ruta: ServiceClient/Domain/Rules/ClientValidationRules.cs
using ServiceClient.Application.Common; // Asegúrate de que el using apunte a tu clase Result
using ServiceClient.Domain.Entities;
using System;
using System.Text.RegularExpressions;

namespace ServiceClient.Domain.Rules
{
    public static class ClientValidationRules
    {
        private static readonly Regex OnlyLettersAndSpacesRegex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]+$");
        private static readonly Regex OnlyNumbersRegex = new Regex("^[0-9]+$");
        private static readonly Regex PhoneRegex = new Regex(@"^\+?[0-9]{8,}$");

        public static Result<Client> Validate(Client client)
        {
            if (client == null)
            {
                return Result<Client>.Failure("El objeto del cliente no puede ser nulo.");
            }

            // Validación del Nombre
            if (string.IsNullOrWhiteSpace(client.Name)) return Result<Client>.Failure("El nombre es obligatorio.");
            if (!OnlyLettersAndSpacesRegex.IsMatch(client.Name)) return Result<Client>.Failure("El nombre solo puede contener letras y espacios.");

            // Validación del Primer Apellido
            if (string.IsNullOrWhiteSpace(client.FirstLastname)) return Result<Client>.Failure("El primer apellido es obligatorio.");
            if (!OnlyLettersAndSpacesRegex.IsMatch(client.FirstLastname)) return Result<Client>.Failure("El primer apellido solo puede contener letras y espacios.");

            // Validación del Segundo Apellido (opcional)
            if (!string.IsNullOrWhiteSpace(client.SecondLastname))
            {
                if (!OnlyLettersAndSpacesRegex.IsMatch(client.SecondLastname))
                {
                    return Result<Client>.Failure("El segundo apellido solo puede contener letras y espacios.");
                }
            }

            // Validación de CI
            if (string.IsNullOrWhiteSpace(client.Ci)) return Result<Client>.Failure("La cédula de identidad es obligatoria.");
            if (!OnlyNumbersRegex.IsMatch(client.Ci)) return Result<Client>.Failure("La cédula de identidad solo puede contener números.");
            // NUEVA REGLA: El CI debe tener exactamente 8 caracteres.
            if (client.Ci.Length != 8) return Result<Client>.Failure("La cédula de identidad debe tener mínimamente 8 caracteres.");

            // Validación de Fecha de Nacimiento
            if (client.DateBirth == null) return Result<Client>.Failure("La fecha de nacimiento es obligatoria.");
            if (!IsAgeValid(client.DateBirth)) return Result<Client>.Failure("La edad del cliente debe estar entre 18 y 80 años.");

            // NUEVA REGLA: El nivel de condición física es obligatorio.
            if (string.IsNullOrWhiteSpace(client.FitnessLevel)) return Result<Client>.Failure("El nivel de condición física es obligatorio.");

            // Validación del Peso Inicial
            if (client.InitialWeightKg == null) return Result<Client>.Failure("El peso inicial es obligatorio.");
            if (client.InitialWeightKg < 30 || client.InitialWeightKg > 300) return Result<Client>.Failure("El peso inicial debe estar entre 30 y 300 kg.");

            // Validación del Peso Actual
            if (client.CurrentWeightKg == null) return Result<Client>.Failure("El peso actual es obligatorio.");
            if (client.CurrentWeightKg.HasValue && (client.CurrentWeightKg <= 0 || client.CurrentWeightKg > 300)) return Result<Client>.Failure("El peso actual, si se especifica, debe ser mayor que 0 y no exceder los 300 kg.");
            

            // Validación del Teléfono de Emergencia (REGLA MODIFICADA)
            if (string.IsNullOrWhiteSpace(client.EmergencyContactPhone)) return Result<Client>.Failure("El teléfono de emergencia es obligatorio.");
            if (!PhoneRegex.IsMatch(client.EmergencyContactPhone)) return Result<Client>.Failure("El teléfono de emergencia debe contener al menos 8 números y no puede contener espacios ni caracteres especiales (excepto un '+' al inicio).");

            return Result<Client>.Success(client);
        }

        // REGLA MODIFICADA: Se ajusta el rango de edad a 18-80 años.
        private static bool IsAgeValid(DateTime? dateBirth)
        {
            if (!dateBirth.HasValue) return false;
            var today = DateTime.Today;
            var age = today.Year - dateBirth.Value.Year;
            if (dateBirth.Value.Date > today.AddYears(-age)) age--;
            return age >= 18 && age <= 80;
        }
    }
}