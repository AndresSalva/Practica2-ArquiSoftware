using GYMPT.Domain;
using GYMPT.Models;

namespace GYMPT.Mappers
{

    public static class UserMapper
    {

        public static T MapToUserDomain<T>(UserData source) where T : User, new()
        {
            if (source == null)
            {
                return null;
            }

            var destination = new T();

            destination.Id = source.Id;
            destination.CreatedAt = source.CreatedAt;
            destination.LastModification = source.LastModification;
            destination.IsActive = source.IsActive;
            destination.Name = source.Name;
            destination.FirstLastname = source.FirstLastname;
            destination.SecondLastname = source.SecondLastname;
            destination.DateBirth = source.DateBirth;
            destination.CI = source.CI;
            destination.Role = source.Role;

            return destination;
        }
    }
}