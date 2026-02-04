using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Auth.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string password, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            return $"{Convert.ToHexString(hash)}-{Convert. ToHexString(salt)}";
        }

        public static bool Verify(string password, string hashedPassword)
        {
            string[] parts = hashedPassword.Split('-');
            byte[] passwordHash = Convert.FromHexString(parts[0]);
            byte[] passwordSalt = Convert.FromHexString(parts[1]);
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
