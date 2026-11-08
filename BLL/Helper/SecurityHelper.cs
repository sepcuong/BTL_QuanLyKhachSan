using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Helper
{
    public class SecurityHelper
    {
        private const int Iterations = 150_000;
        private const int HashSize = 32;

        public static string HashPassword(string input, string salt)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt ?? string.Empty);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, saltBytes, Iterations, HashAlgorithmName.SHA256, HashSize);
            return $"PBKDF2${Iterations}${Convert.ToBase64String(saltBytes)}${Convert.ToBase64String(hash)}";
        }

        public static string HashLegacyPassword(string input)
        {
            return Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(input))).ToLowerInvariant();
        }

        public static bool VerifyPassword(string input, string storedHash, string salt)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            var parts = storedHash.Split('$');
            if (parts.Length == 4 && parts[0] == "PBKDF2" &&
                int.TryParse(parts[1], out var iterations))
            {
                try
                {
                    var storedSalt = Convert.FromBase64String(parts[2]);
                    var expectedHash = Convert.FromBase64String(parts[3]);
                    var actualHash = Rfc2898DeriveBytes.Pbkdf2(input, storedSalt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);
                    return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            var legacyMd5 = HashLegacyPassword(input);
            return string.Equals(storedHash, legacyMd5, StringComparison.OrdinalIgnoreCase) || storedHash == input;
        }
    }
}
