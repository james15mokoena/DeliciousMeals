using System.Security.Cryptography;

namespace DeliciousMeals.Security
{
    public static class PasswordEncrypter
    {

        public static string HashPassword(string password)
        {
            // Generate a random salt.
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            // Create a hash using PBKDF2
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 970, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine salt and hash.
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Convert the combined salt and hash to a string for storage.
            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public static bool VerifyPassword(string customerPassword, string hashedPassword)
        {
            // Convert the stored hash string back to bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Extract the salt from the stored hash.
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash of the input password using the stored salt;
            var pbkdf2 = new Rfc2898DeriveBytes(customerPassword, salt, 970, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the computed hash with the stored hash.
            for (int i = 0; i < 20; ++i)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }


    }
}
