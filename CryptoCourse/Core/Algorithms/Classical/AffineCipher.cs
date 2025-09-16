using CryptoCourse.Utils; // Import our new helper library
using System;
using System.Text;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class AffineCipher
    {
        private const int AlphabetSize = 26;

        public static string Encrypt(string plainText, int a, int b)
        {
            if (MathHelper.Gcd(a, AlphabetSize) != 1)
            {
                throw new ArgumentException($"Key 'a' ({a}) must be coprime with {AlphabetSize}.");
            }

            var result = new StringBuilder();
            foreach (char c in plainText)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int p = c - offset;
                    int encrypted = (a * p + b) % AlphabetSize;
                    result.Append((char)(encrypted + offset));
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }

        public static string Decrypt(string cipherText, int a, int b)
        {
            if (MathHelper.Gcd(a, AlphabetSize) != 1)
            {
                throw new ArgumentException($"Key 'a' ({a}) must be coprime with {AlphabetSize} to be invertible.");
            }

            int a_inv = MathHelper.ModInverse(a, AlphabetSize);
            var result = new StringBuilder();

            foreach (char c in cipherText)
            {
                if (char.IsLetter(c))
                {
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int ct = c - offset;
                    // Use our custom Mod function to handle potential negative numbers from (ct - b)
                    int decrypted = MathHelper.Mod(a_inv * (ct - b), AlphabetSize);
                    result.Append((char)(decrypted + offset));
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}