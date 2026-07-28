using System.Security.Cryptography;
using System.Text;

namespace AuthUsuariosMicroservice.Services
{
    public interface IPasswordHasherService
    {
        void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }

    public class PasswordHasherService : IPasswordHasherService
    {
        public void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key; // Genera Salt de 128 bytes
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Genera Hash de 64 bytes
        }

        public bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // 1. Verificación por HMACSHA512 (Usuarios creados desde C# / API)
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                if (computedHash.SequenceEqual(passwordHash))
                    return true;
            }

            // 2. Verificación por SHA512 de SQL (Compatibilidad con el script SQL inicial)
            using (var sha512 = SHA512.Create())
            {
                // SQL hizo: HASHBYTES('SHA2_512', CONCAT(Password, SaltAsString))
                string saltString = Encoding.UTF8.GetString(passwordSalt);
                var combinedBytes = Encoding.UTF8.GetBytes(password + saltString);
                var sqlHash = sha512.ComputeHash(combinedBytes);

                if (sqlHash.SequenceEqual(passwordHash))
                    return true;
            }

            return false;
        }
    }
}