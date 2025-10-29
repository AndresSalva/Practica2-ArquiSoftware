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
            return Result<Person>.Success(person);
        }
        }
}
