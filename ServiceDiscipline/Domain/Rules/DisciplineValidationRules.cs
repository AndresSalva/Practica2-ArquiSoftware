using ServiceDiscipline.Application.Common;
using ServiceDiscipline.Domain.Entities;
using ServiceDiscipline.Domain.Ports;
using ServiceDiscipline.Domain.Rules;
using static System.Net.Mime.MediaTypeNames;

namespace ServiceDiscipline.Domain.Rules
{
    public static class DisciplineValidationRules
    {
        public static Result<Discipline> Validate(Discipline discipline)
        {
            if (discipline == null)
            {
                return Result<Discipline>.Failure("La disciplina no puede ser nula.");
            }

            if (string.IsNullOrWhiteSpace(discipline.Name))
            {
                return Result<Discipline>.Failure("El nombre de la disciplina es obligatorio.");
            }

            if (discipline.Name.Length > 100)
            {
                return Result<Discipline>.Failure("El nombre de la disciplina no puede exceder los 100 caracteres.");
            }

            // Si todas las validaciones pasan, devolvemos un resultado exitoso.
            return Result<Discipline>.Success(discipline);
        }
    }
}