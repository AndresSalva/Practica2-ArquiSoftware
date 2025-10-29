using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ServiceUser.Domain.Rules
{
    internal class UserValidationRules
    {
            // Nombre completo obligatorio, mínimo 2 letras, solo letras y espacios
            public static bool EsNombreCompletoValido(string? nombreCompleto)
            {
                if (string.IsNullOrWhiteSpace(nombreCompleto)) return false;
                if (nombreCompleto.Length < 2) return false;
                return Regex.IsMatch(nombreCompleto, @"^[A-Za-zÁÉÍÓÚáéíóúÑñ ]+$");
            }

            // CI solo números o letras, opcional
            public static bool EsCiValido(string? ci)
            {
                if (string.IsNullOrWhiteSpace(ci)) return true; // Opcional
                return Regex.IsMatch(ci, @"^[0-9A-Za-z]+$");
            }

            // Fecha de nacimiento no futura
            public static bool EsFechaNacimientoValida(DateTime? fecha)
            {
                if (fecha == null) return true;
                return fecha <= DateTime.Today;
            }

            // Fecha de contratación no futura
            public static bool EsFechaContratacionValida(DateTime? hireDate, DateTime? dateBirth)
            {
                if (!hireDate.HasValue || !dateBirth.HasValue)
                    return false; // Si alguno es null, no es válido

                DateTime fechaContratacion = hireDate.Value;
                DateTime nacimiento = dateBirth.Value;

                // No puede ser en el futuro
                if (fechaContratacion > DateTime.Today)
                    return false;

                // Debe ser posterior a la fecha de nacimiento
                if (fechaContratacion <= nacimiento)
                    return false;

                // Debe tener al menos 18 años
                int edadAlContratar = fechaContratacion.Year - nacimiento.Year;
                if (fechaContratacion < nacimiento.AddYears(edadAlContratar))
                    edadAlContratar--;

                return edadAlContratar >= 18;
            }


            // Rol válido
            public static bool EsRoleValido(string? role)
            {
                if (string.IsNullOrWhiteSpace(role)) return false;
                string[] rolesValidos = { "Client", "Instructor", "Admin" };
                return Array.Exists(rolesValidos, r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
            }

            // Especialización (solo para Instructor), opcional pero mínimo 3 caracteres si existe
            public static bool EsEspecializacionValida(string? especializacion)
            {
                if (string.IsNullOrWhiteSpace(especializacion)) return true;
                return especializacion.Length >= 3;
            }

            // Peso inicial y peso actual (solo Client)
            public static bool EsPesoValido(decimal? peso)
            {
                if (peso == null) return true;
                return peso > 0;
            }

            // Peso actual ≥ peso inicial
            public static bool EsPesoActualValido(decimal? pesoInicial, decimal? pesoActual)
            {
                if (pesoActual == null || pesoInicial == null) return true;
                return pesoActual >= pesoInicial;
            }

            // Nivel de fitness
            public static bool EsNivelFitnessValido(string nivel)
            {
                return nivel == "Principiante" || nivel == "Intermedio" || nivel == "Avanzado";
            }

            // Teléfono de emergencia (solo números, mínimo 7 dígitos)
            public static bool EsTelefonoEmergenciaValido(string? telefono)
            {
                if (string.IsNullOrWhiteSpace(telefono)) return true;
                return Regex.IsMatch(telefono, @"^\d{7,}$");
            }

            public static bool EsSalarioValido(decimal? salario)
            {
                if (!salario.HasValue) return false;
                // Salario mínimo 0
                return salario.Value >= 0;
            }
            public static bool EsTelefonoValido(string telefono)
            {
                if (string.IsNullOrWhiteSpace(telefono)) return false;
                return telefono.All(char.IsDigit) && telefono.Length >= 7; // mínimo 7 dígitos
            }

    }
}
