using GYMPT.Domain;

namespace GYMPT.Factories
{

    public abstract class UserCreator
    {

        public abstract User CreateUser();

        public string GetUserInfo()
        {

            var user = CreateUser();

            return $"Se ha creado un objeto para el usuario con ID: {user.Id} y rol: {user.Role}.";
        }
    }
}