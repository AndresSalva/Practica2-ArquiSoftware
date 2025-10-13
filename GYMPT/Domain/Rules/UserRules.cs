using System;
using System.Text.RegularExpressions;

namespace GYMPT.Domain.Rules
{
    public static class UserRules
    {
        /// <summary>
        /// Valida el nombre completo o apellidos
        /// </summary>
        public static bool NombreCompletoValido(string? nombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto)) return false;
            if (nombreCompleto.Length < 2) return false;
            return Regex.IsMatch(nombreCompleto, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$");
        }

        /// <summary>
        /// Valida el CI (opcional)
        /// </summary>
        public static bool CiValido(string? ci)
        {
            if (string.IsNullOrWhiteSpace(ci)) return true; // opcional
            return Regex.IsMatch(ci, @"^[0-9A-Za-z]+$");
        }

        /// <summary>
        /// Valida que la fecha de nacimiento no sea futura
        /// </summary>
        public static bool FechaNacimientoValida(DateTime? fecha)
        {
            if (!fecha.HasValue) return true;
            return fecha.Value <= DateTime.Today;
        }

        /// <summary>
        /// Valida que el rol sea uno de los permitidos
        /// </summary>
        public static bool RoleValido(string? role)
        {
            if (string.IsNullOrWhiteSpace(role)) return false;
            string[] rolesValidos = { "Client", "Instructor", "Admin" };
            return Array.Exists(rolesValidos, r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Valida que el usuario sea mayor de edad (edad mínima configurable)
        /// </summary>
        public static bool EsMayorDeEdad(DateTime? fechaNacimiento, int edadMinima = 18)
        {
            if (!fechaNacimiento.HasValue) return false;
            DateTime hoy = DateTime.Today;
            int edad = hoy.Year - fechaNacimiento.Value.Year;
            if (fechaNacimiento.Value > hoy.AddYears(-edad)) edad--;
            return edad >= edadMinima;
        }

        /// <summary>
        /// Valida todos los campos de usuario y retorna un mensaje de error (vacío si es válido)
        /// </summary>
        public static string ValidarUsuario(string? nombre, string? primerApellido, string? segundoApellido,
                                            string? ci, DateTime? fechaNacimiento, string? role)
        {
            if (!NombreCompletoValido(nombre))
                return "Nombre no válido.";
            if (!NombreCompletoValido(primerApellido))
                return "Primer apellido no válido.";
            if (!NombreCompletoValido(segundoApellido))
                return "Segundo apellido no válido.";
            if (!CiValido(ci))
                return "CI no válido.";
            if (!FechaNacimientoValida(fechaNacimiento))
                return "Fecha de nacimiento inválida.";
            if (!EsMayorDeEdad(fechaNacimiento))
                return "El usuario debe ser mayor de edad para registrarse.";
            if (!RoleValido(role))
                return "Rol no válido.";

            return string.Empty; // Todo válido
        }
    }
}
