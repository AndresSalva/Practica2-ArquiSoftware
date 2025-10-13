using System;
using System.Linq;

namespace GYMPT.Domain.Rules
{
    public static class ClientRules
    {
        public static bool PesoInicialValido(decimal? pesoInicial)
        {
            if (!pesoInicial.HasValue) return true; // opcional
            return pesoInicial.Value > 0;
        }

        public static bool PesoActualValido(decimal? pesoInicial, decimal? pesoActual)
        {
            if (!pesoInicial.HasValue || !pesoActual.HasValue) return true;
            return pesoActual.Value >= pesoInicial.Value;
        }

        public static bool NivelFitnessValido(string? nivel)
        {
            if (string.IsNullOrWhiteSpace(nivel)) return false;
            string[] nivelesValidos = { "Principiante", "Intermedio", "Avanzado" };
            return Array.Exists(nivelesValidos, n => n.Equals(nivel, StringComparison.OrdinalIgnoreCase));
        }

        public static bool TelefonoEmergenciaValido(string? telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono)) return true; // opcional
            return telefono.Length >= 7 && telefono.All(char.IsDigit);
        }

        public static bool FechaNacimientoValida(DateTime? fechaNacimiento, int edadMinima = 0)
        {
            if (!fechaNacimiento.HasValue) return true;
            DateTime hoy = DateTime.Today;
            if (fechaNacimiento.Value > hoy) return false;

            if (edadMinima > 0)
            {
                int edad = hoy.Year - fechaNacimiento.Value.Year;
                if (fechaNacimiento.Value > hoy.AddYears(-edad)) edad--;
                return edad >= edadMinima;
            }

            return true;
        }
    }
}
