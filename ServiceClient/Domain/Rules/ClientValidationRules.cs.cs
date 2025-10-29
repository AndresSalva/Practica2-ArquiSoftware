using ServiceClient.Domain.Entities;
using ServiceCommon.Application;
using System.Text.RegularExpressions;

namespace ServiceClient.Domain.Rules
{
    public static class ClientValidationRules
    {
        private static readonly Regex OnlyLettersAndSpacesRegex = new Regex("^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ ]+$");
        private static readonly Regex OnlyNumbersRegex = new Regex("^[0-9]+$");
        private static readonly Regex PhoneRegex = new Regex(@"^\+?[0-9]+$");

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
            if (!string.IsNullOrEmpty(client.SecondLastname) && !OnlyLettersAndSpacesRegex.IsMatch(client.SecondLastname))
            {
                return Result<Client>.Failure("El segundo apellido solo puede contener letras y espacios.");
            }

            // Validación de CI
            if (string.IsNullOrWhiteSpace(client.Ci)) return Result<Client>.Failure("La cédula de identidad es obligatoria.");
            if (!OnlyNumbersRegex.IsMatch(client.Ci)) return Result<Client>.Failure("La cédula de identidad solo puede contener números y no puede ser negativa.");

            // Validación de Fecha de Nacimiento (Edad entre 18 y 50)
            if (client.DateBirth == null) return Result<Client>.Failure("La fecha de nacimiento es obligatoria.");
            if (!IsAgeValid(client.DateBirth)) return Result<Client>.Failure("La edad del cliente debe estar entre 18 y 50 años.");

            // Validación del Peso Inicial
            if (client.InitialWeightKg == null) return Result<Client>.Failure("El peso inicial es obligatorio.");
            if (client.InitialWeightKg < 30 || client.InitialWeightKg > 300) return Result<Client>.Failure("El peso inicial debe estar entre 30 y 300 kg.");

            // Validación del Peso Actual (opcional)
            if (client.CurrentWeightKg.HasValue && client.CurrentWeightKg < 0)
            {
                return Result<Client>.Failure("El peso actual no puede ser un número negativo.");
            }

            // Validación del Teléfono de Emergencia
            if (string.IsNullOrWhiteSpace(client.EmergencyContactPhone)) return Result<Client>.Failure("El teléfono de emergencia es obligatorio.");
            if (!PhoneRegex.IsMatch(client.EmergencyContactPhone)) return Result<Client>.Failure("El formato del teléfono de emergencia no es válido. Solo puede contener números.");

            return Result<Client>.Success(client);
        }

        private static bool IsAgeValid(DateTime? dateBirth)
        {
            if (!dateBirth.HasValue) return false;
            var today = DateTime.Today;
            var age = today.Year - dateBirth.Value.Year;
            if (dateBirth.Value.Date > today.AddYears(-age)) age--;
            return age >= 18 && age <= 50;
        }
    }
}