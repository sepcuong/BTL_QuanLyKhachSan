using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Helper
{
    public class SecurityHelper
    {
        public static string HashPassword(string input, string salt)
        {
            return HashLegacyPassword(input);
        }

        public static string HashLegacyPassword(string input)
        {
            return Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(input))).ToLowerInvariant();
        }

        public static bool VerifyPassword(string input, string storedHash, string salt)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            var md5Hash = HashPassword(input, salt);
            return string.Equals(storedHash, md5Hash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
