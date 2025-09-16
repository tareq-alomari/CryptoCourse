using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptoCourse.Core.Algorithms.Modern.SecureWrappers
{
    public static class RsaWrapper
    {
        public static void GenerateKeys(out string publicKey, out string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048)) // 2048-bit key is secure
            {
                publicKey = rsa.ToXmlString(false);  // Export public key
                privateKey = rsa.ToXmlString(true); // Export private key
            }
        }

        public static string Encrypt(string plainText, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(publicKey);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = rsa.Encrypt(plainBytes, true); // Use OAEP padding
                return Convert.ToBase64String(cipherBytes);
            }
        }

        public static string Decrypt(string cipherText, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privateKey);
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] plainBytes = rsa.Decrypt(cipherBytes, true); // Use OAEP padding
                return Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}