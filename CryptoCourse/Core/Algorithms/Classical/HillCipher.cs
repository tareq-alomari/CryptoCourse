using System;
using System.Linq;
using System.Text;
using CryptoCourse.Utils;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class HillCipher
    {
        private const string EnglishAlphabet = "abcdefghijklmnopqrstuvwxyz";
        private const string ArabicAlphabet = "ابتثجحخدذرزسشصضطظعغفقكلمنهوي";

        // FIX 1: Helper function to detect language and return the correct alphabet
        private static string GetAlphabet(char c, out int modulus)
        {
            if (ArabicAlphabet.Contains(char.ToLower(c)))
            {
                modulus = ArabicAlphabet.Length;
                return ArabicAlphabet;
            }
            if (char.IsLetter(c))
            {
                modulus = EnglishAlphabet.Length;
                return EnglishAlphabet;
            }
            modulus = 0;
            return null;
        }

        private static int[,] CreateKeyMatrix(string key, string alphabet)
        {
            int m = (int)Math.Sqrt(key.Length);
            if (m * m != key.Length) throw new ArgumentException("طول المفتاح يجب أن يكون مربعًا كاملاً.");

            var matrix = new int[m, m];
            int k = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    int index = alphabet.IndexOf(char.ToLower(key[k++]));
                    if (index == -1) throw new ArgumentException($"المفتاح يحتوي على حروف غير موجودة في الأبجدية.");
                    matrix[i, j] = index;
                }
            }
            return matrix;
        }

        private static string Process(string text, string key, bool isEncrypt)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(key)) return text;

            int m = (int)Math.Sqrt(key.Length);
            if (m * m != key.Length) throw new ArgumentException("طول المفتاح يجب أن يكون مربعًا كاملاً.");

            var result = new StringBuilder();

            for (int i = 0; i < text.Length; i += m)
            {
                string block = text.Substring(i, Math.Min(m, text.Length - i));

                // Determine language from the first character of the block
                var alphabet = GetAlphabet(block[0], out int modulus);
                if (alphabet == null) // If block starts with a non-alphabetic char, skip it
                {
                    result.Append(block);
                    continue;
                }

                // FIX 2: Pad the block if it's shorter than m, using a letter from the correct alphabet
                while (block.Length < m)
                {
                    block += (alphabet == ArabicAlphabet) ? 'ي' : 'X';
                }

                // Create matrices and vectors using the detected alphabet
                var keyMatrix = CreateKeyMatrix(key, alphabet);
                var processMatrix = isEncrypt ? keyMatrix : MatrixHelper.InverseMatrix(keyMatrix, modulus);

                var vector = new int[m];
                var resultVector = new int[m];
                bool[] isUpper = new bool[m]; // Keep track of original case

                for (int j = 0; j < m; j++)
                {
                    isUpper[j] = char.IsUpper(block[j]);
                    vector[j] = alphabet.IndexOf(char.ToLower(block[j]));
                }

                for (int row = 0; row < m; row++)
                {
                    int sum = 0;
                    for (int col = 0; col < m; col++)
                    {
                        sum += processMatrix[row, col] * vector[col];
                    }
                    resultVector[row] = MathHelper.Mod(sum, modulus);
                }

                for (int j = 0; j < m; j++)
                {
                    char processedChar = alphabet[resultVector[j]];
                    // FIX 3: Restore the original case for English letters
                    result.Append(isUpper[j] && alphabet == EnglishAlphabet ? char.ToUpper(processedChar) : processedChar);
                }
            }

            return result.ToString();
        }

        public static string Encrypt(string plainText, string key)
        {
            return Process(plainText, key, true);
        }

        public static string Decrypt(string cipherText, string key)
        {
            return Process(cipherText, key, false);
        }
    }
}