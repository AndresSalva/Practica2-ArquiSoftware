namespace GYMPT.Application.Interfaces
{
    public interface IPasswordHasher
    {
        // Devuelve el string que guardarás en la BD
        string HashPassword(string password);

        // Verifica si la contraseña ingresada coincide con el hash guardado
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}
