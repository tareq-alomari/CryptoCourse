using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptoCourse.Core.Algorithms.Modern.SecureWrappers
{
    public static class AesWrapper
    {
        // We use a fixed salt for simplicity. In a real application, this should be unique per user/data.
        private static readonly byte[] Salt = Encoding.UTF8.GetBytes("SomeFixedSaltValue");

        public static string Encrypt(string plainText, string password)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            using (var aes = Aes.Create())
            {
                // Derive a key and IV from the password using PBKDF2
                var key = new Rfc2898DeriveBytes(password, Salt, 10000, HashAlgorithmName.SHA256);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string password)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(password, Salt, 10000, HashAlgorithmName.SHA256);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                    }
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
        }
    }
}