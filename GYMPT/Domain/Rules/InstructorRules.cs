using System;
using GYMPT.Domain.Shared;

namespace GYMPT.Domain.Rules
{
    public static class InstructorRules
    {
        public static Result EsFechaContratacionValida(DateTime? hireDate, DateTime? dateBirth)
        {
            if (!hireDate.HasValue || !dateBirth.HasValue)
                return Result.Fail("Fechas requeridas.");

            DateTime contratacion = hireDate.Value;
            DateTime nacimiento = dateBirth.Value;

            if (contratacion > DateTime.Today)
                return Result.Fail("La fecha de contratación no puede ser futura.");

            if (contratacion <= nacimiento)
                return Result.Fail("La fecha de contratación no puede ser anterior al nacimiento.");

            int edadAlContratar = contratacion.Year - nacimiento.Year;
            if (contratacion < nacimiento.AddYears(edadAlContratar)) edadAlContratar--;

            if (edadAlContratar < 18)
                return Result.Fail("El instructor debe tener al menos 18 años al contratar.");

            return Result.Ok();
        }

        public static Result EsEspecializacionValida(string? especializacion)
        {
            if (string.IsNullOrWhiteSpace(especializacion))
                return Result.Fail("Especialización requerida.");

            if (especializacion.Length < 3)
                return Result.Fail("Especialización debe tener al menos 3 caracteres.");

            return Result.Ok();
        }

        public static Result EsSalarioValido(decimal? salario)
        {
            if (!salario.HasValue)
                return Result.Fail("Salario requerido.");

            if (salario.Value < 0)
                return Result.Fail("Salario no puede ser negativo.");

            return Result.Ok();
        }
    }
}
