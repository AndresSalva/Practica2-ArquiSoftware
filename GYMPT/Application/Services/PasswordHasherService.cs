using GYMPT.Application.Interfaces;
using System;
using System.Security.Cryptography;
using System.Text;
namespace GYMPT.Application.Services
{
    public class PasswordHasherService : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32; 
        private const int Iterations = 100_000;

        public string HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                string result = $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
                return result;
            }
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null) throw new ArgumentNullException(nameof(hashedPassword));
            if (providedPassword == null) throw new ArgumentNullException(nameof(providedPassword));

            var parts = hashedPassword.Split('.', 3);
            if (parts.Length != 3) return false;

            if (!int.TryParse(parts[0], out int iterations)) return false;
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] storedHash = Convert.FromBase64String(parts[2]);

            using (var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, iterations, HashAlgorithmName.SHA256))
            {
                byte[] computedHash = pbkdf2.GetBytes(storedHash.Length);

                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
            }
        }
    }
}
