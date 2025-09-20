using CryptoCourse.Utils; // Import our helper library
using System;
using System.Text;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class AffineCipher
    {
        private const string EnglishAlphabet = "abcdefghijklmnopqrstuvwxyz";
        private const string ArabicAlphabet = "ابتثجحخدذرزسشصضطظعغفقكلمنهوي";

        public static string Encrypt(string plainText, int a, int b)
        {
            var result = new StringBuilder();
            foreach (char c in plainText)
            {
                int arabicIndex = ArabicAlphabet.IndexOf(char.ToLower(c));
                if (arabicIndex != -1) // Arabic character
                {
                    if (MathHelper.Gcd(a, ArabicAlphabet.Length) != 1)
                        throw new ArgumentException($"المفتاح 'a' ({a}) يجب ألا يكون له قواسم مشتركة مع حجم الأبجدية العربية ({ArabicAlphabet.Length}).");

                    int p = arabicIndex;
                    int encrypted = (a * p + b) % ArabicAlphabet.Length;
                    result.Append(ArabicAlphabet[encrypted]);
                }
                else if (char.IsLetter(c)) // English character
                {
                    if (MathHelper.Gcd(a, EnglishAlphabet.Length) != 1)
                        throw new ArgumentException($"المفتاح 'a' ({a}) يجب ألا يكون له قواسم مشتركة مع حجم الأبجدية الإنجليزية ({EnglishAlphabet.Length}).");

                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int p = c - offset;
                    int encrypted = (a * p + b) % EnglishAlphabet.Length;
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
            var result = new StringBuilder();
            foreach (char c in cipherText)
            {
                int arabicIndex = ArabicAlphabet.IndexOf(char.ToLower(c));
                if (arabicIndex != -1) // Arabic character
                {
                    if (MathHelper.Gcd(a, ArabicAlphabet.Length) != 1)
                        throw new ArgumentException($"المفتاح 'a' ({a}) غير صالح لفك التشفير.");

                    int a_inv = MathHelper.ModInverse(a, ArabicAlphabet.Length);
                    int ct = arabicIndex;
                    int decrypted = MathHelper.Mod(a_inv * (ct - b), ArabicAlphabet.Length);
                    result.Append(ArabicAlphabet[decrypted]);
                }
                else if (char.IsLetter(c)) // English character
                {
                    if (MathHelper.Gcd(a, EnglishAlphabet.Length) != 1)
                        throw new ArgumentException($"المفتاح 'a' ({a}) غير صالح لفك التشفير.");

                    int a_inv = MathHelper.ModInverse(a, EnglishAlphabet.Length);
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int ct = c - offset;
                    int decrypted = MathHelper.Mod(a_inv * (ct - b), EnglishAlphabet.Length);
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