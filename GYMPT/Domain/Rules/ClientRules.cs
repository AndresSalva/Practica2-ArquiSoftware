using System;
using System.Linq;
using GYMPT.Domain.Shared;

namespace GYMPT.Domain.Rules
{
    public static class ClientRules
    {
        public static Result PesoInicialValido(decimal? pesoInicial)
        {
            if (!pesoInicial.HasValue) return Result.Ok();
            if (pesoInicial.Value <= 0) return Result.Fail("El peso inicial debe ser mayor a 0.");
            return Result.Ok();
        }

        public static Result PesoActualValido(decimal? pesoInicial, decimal? pesoActual)
        {
            if (!pesoInicial.HasValue || !pesoActual.HasValue) return Result.Ok();
            if (pesoActual.Value < pesoInicial.Value) return Result.Fail("El peso actual no puede ser menor que el peso inicial.");
            return Result.Ok();
        }

        public static Result NivelFitnessValido(string? nivel)
        {
            if (string.IsNullOrWhiteSpace(nivel)) return Result.Fail("Nivel de fitness requerido.");
            string[] nivelesValidos = { "Principiante", "Intermedio", "Avanzado" };
            if (!nivelesValidos.Contains(nivel, StringComparer.OrdinalIgnoreCase))
                return Result.Fail("Nivel de fitness inválido.");
            return Result.Ok();
        }

        public static Result TelefonoEmergenciaValido(string? telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono)) return Result.Ok();
            if (telefono.Length < 7 || !telefono.All(char.IsDigit))
                return Result.Fail("Teléfono inválido (mínimo 7 dígitos).");
            return Result.Ok();
        }

        public static Result FechaNacimientoValida(DateTime? fechaNacimiento, int edadMinima = 0)
        {
            if (!fechaNacimiento.HasValue) return Result.Ok();

            DateTime hoy = DateTime.Today;
            if (fechaNacimiento.Value > hoy) return Result.Fail("Fecha de nacimiento no puede ser futura.");

            if (edadMinima > 0)
            {
                int edad = hoy.Year - fechaNacimiento.Value.Year;
                if (fechaNacimiento.Value > hoy.AddYears(-edad)) edad--;
                if (edad < edadMinima) return Result.Fail($"Debe tener al menos {edadMinima} años.");
            }

            return Result.Ok();
        }
    }
}
