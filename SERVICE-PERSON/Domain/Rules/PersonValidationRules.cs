using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ServicePerson.Domain.Entities;
using ServicePerson.Application.Common;

namespace ServicePerson.Domain.Rules
{
    internal class PersonValidationRules
    {
        private static readonly Regex AllowedCharsRegex = new Regex("^[a-zA-Z0-9 ñáéíóúÁÉÍÓÚüÜ]+$");

        public static Result<Person> Validate(Person person)
        {
            if (person == null)
            {
                return Result<Person>.Failure("Persona no puede quedar vacía.");
            }
            if (string.IsNullOrWhiteSpace(person.Name))
            {
                return Result<Person>.Failure("El nombre de la persona es obligatorio.");
            }
            if (!AllowedCharsRegex.IsMatch(person.Name))
            {
                return Result<Person>.Failure("El nombre de la persona contiene caracteres no permitidos.");
            }
            if (string.IsNullOrWhiteSpace(person.FirstLastname))
                return Result<Person>.Failure("El primer apellido es obligatorio.");
            if (!AllowedCharsRegex.IsMatch(person.FirstLastname))
                return Result<Person>.Failure("El primer apellido contiene caracteres no permitidos.");
            if (person.FirstLastname.Length < 2 || person.FirstLastname.Length > 50)
                return Result<Person>.Failure("El primer apellido debe tener entre 2 y 50 caracteres.");
            if (!AllowedCharsRegex.IsMatch(person.SecondLastname))
                return Result<Person>.Failure("El segundo apellido contiene caracteres no permitidos.");
            if (string.IsNullOrWhiteSpace(person.Ci))
                return Result<Person>.Failure("El número de CI es obligatorio.");
            if (!Regex.IsMatch(person.Ci, @"^[A-Za-z0-9]+$"))
                return Result<Person>.Failure("El número de CI solo puede contener letras y números.");
            if (person.Ci.Length < 6 || person.Ci.Length > 15)
                return Result<Person>.Failure("El número de CI debe tener entre 6 y 15 caracteres.");
            if (person.DateBirth == null)
                return Result<Person>.Failure("La fecha de nacimiento es obligatoria.");
            if (person.DateBirth > DateTime.Now)
                return Result<Person>.Failure("La fecha de nacimiento no puede ser futura.");
            if (person.CreatedAt != null && person.CreatedAt > DateTime.Now)
                return Result<Person>.Failure("La fecha de creación no puede ser futura.");
            if (person.LastModification != null && person.CreatedAt != null &&
                person.LastModification < person.CreatedAt)
                return Result<Person>.Failure("La fecha de última modificación no puede ser anterior a la creación.");
            return Result<Person>.Success(person);
        }
        }
}
