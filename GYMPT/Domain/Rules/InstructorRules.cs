using System;

namespace GYMPT.Domain.Rules
{
    public static class InstructorRules
    {
        public static bool EsFechaContratacionValida(DateTime? hireDate, DateTime? dateBirth)
        {
            if (!hireDate.HasValue || !dateBirth.HasValue)
                return false;

            DateTime contratacion = hireDate.Value;
            DateTime nacimiento = dateBirth.Value;

            if (contratacion > DateTime.Today) return false;     // No puede ser en el futuro
            if (contratacion <= nacimiento) return false;       // No puede ser antes del nacimiento

            // Debe tener al menos 18 años en la fecha de contratación
            int edadAlContratar = contratacion.Year - nacimiento.Year;
            if (contratacion < nacimiento.AddYears(edadAlContratar)) edadAlContratar--;

            return edadAlContratar >= 18;
        }

        public static bool EsEspecializacionValida(string? especializacion)
        {
            if (string.IsNullOrWhiteSpace(especializacion)) return false;
            return especializacion.Length >= 3;
        }

        public static bool EsSalarioValido(decimal? salario)
        {
            if (!salario.HasValue) return false;
            return salario.Value >= 0;
        }
    }
}
