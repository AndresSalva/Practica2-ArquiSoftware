using System;
using System.Text.RegularExpressions;
using GYMPT.Domain.Shared;

namespace GYMPT.Domain.Rules
{
    public static class UserRules
    {
        public static Result NombreCompletoValido(string? nombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                return Result.Fail("El nombre no puede estar vacío.");

            if (nombreCompleto.Length < 2)
                return Result.Fail("El nombre debe tener al menos 2 caracteres.");

            if (!Regex.IsMatch(nombreCompleto, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$"))
                return Result.Fail("El nombre solo puede contener letras y espacios.");

            return Result.Ok();
        }   

        public static Result CiValido(string? ci)
        {
            if (string.IsNullOrWhiteSpace(ci)) return Result.Ok(); // opcional

            if (!Regex.IsMatch(ci, @"^[0-9A-Za-z]+$"))
                return Result.Fail("CI inválido. Solo números y letras.");

            return Result.Ok();
        }

        public static Result FechaNacimientoValida(DateTime? fecha)
        {
            if (!fecha.HasValue) return Result.Ok();

            if (fecha.Value > DateTime.Today)
                return Result.Fail("Fecha de nacimiento no puede ser futura.");

            return Result.Ok();
        }

        public static Result RoleValido(string? role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return Result.Fail("El rol no puede estar vacío.");

            string[] rolesValidos = { "Client", "Instructor", "Admin" };
            if (!Array.Exists(rolesValidos, r => r.Equals(role, StringComparison.OrdinalIgnoreCase)))
                return Result.Fail("Rol inválido.");

            return Result.Ok();
        }

        public static Result EsMayorDeEdad(DateTime? fechaNacimiento, int edadMinima = 18)
        {
            if (!fechaNacimiento.HasValue)
                return Result.Fail("Fecha de nacimiento requerida.");

            DateTime hoy = DateTime.Today;
            int edad = hoy.Year - fechaNacimiento.Value.Year;
            if (fechaNacimiento.Value > hoy.AddYears(-edad)) edad--;

            if (edad < edadMinima)
                return Result.Fail($"Debe tener al menos {edadMinima} años.");

            return Result.Ok();
        }
    }
}
