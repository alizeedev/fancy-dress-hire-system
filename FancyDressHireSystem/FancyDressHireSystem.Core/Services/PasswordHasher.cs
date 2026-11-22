using System.Security.Cryptography;
using System.Text;
namespace FancyDressHireSystem.Core.Services
{
    ///<summary>
    ///Hashes and checks passwords using PBKDF2
    ///Settings: SHA256,100000 iterations, 16-byte salt, 32-byte hash,stored as Base64.
    ///References:Microsoft Learn, "Rfc2898DeriveBytes.Pbkdf2 Method"
    ///</summary>
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        ///<summary>
        ///Creates a random salt.
        ///</summary>
        ///<returns>The salt is Base64 text.</returns>
       
        public static string GenerateSalt()
        {

            byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
            return Convert.ToBase64String(saltBytes);
        }

        ///<summary>
        ///Hashes the password with the salt
        ///</summary>
        ///<returns>The hash is Base64 text.</returns>
        public static string Hash(string password,string salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Convert.FromBase64String(salt);
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(passwordBytes, saltBytes, Iterations, HashAlgorithmName.SHA256, HashSize);
            return Convert.ToBase64String(hashBytes);
        }
        ///<summary>
        ///Check whether typed password matches the stored hash and salt
        ///</summary>
        ///<returns>True if password is correct and False if incorrect</returns>
        public static bool Verify(string password, string storedHash, string salt)
        {
            if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(salt))
                return false;
            string typedHash = Hash(password, salt);
            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(typedHash),
                Convert.FromBase64String(storedHash));
           
        }


    }
}