using System;
using System.Text;
using CryptoCourse.Utils;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class HillCipher
    {
        private const int AlphabetSize = 26;

        private static int[,] CreateKeyMatrix(string key)
        {
            int len = key.Length;
            int m = (int)Math.Sqrt(len);
            if (m * m != len)
            {
                throw new ArgumentException("Key length must be a perfect square (4, 9, 16, etc.).");
            }

            int[,] matrix = new int[m, m];
            int k = 0;
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = (key.ToUpper()[k]) - 'A';
                    k++;
                }
            }
            return matrix;
        }

        private static string Process(string text, string key, bool isEncrypt)
        {
            int[,] keyMatrix = CreateKeyMatrix(key);
            int m = keyMatrix.GetLength(0);

            // For decryption, we need the inverse of the key matrix
            int[,] processMatrix = isEncrypt ? keyMatrix : MatrixHelper.InverseMatrix(keyMatrix, AlphabetSize);

            // Pad the text with 'X' to be a multiple of m
            while (text.Length % m != 0)
            {
                text += 'X';
            }

            var result = new StringBuilder();
            text = text.ToUpper();

            for (int i = 0; i < text.Length; i += m)
            {
                int[] vector = new int[m];
                for (int j = 0; j < m; j++)
                {
                    vector[j] = text[i + j] - 'A';
                }

                int[] resultVector = new int[m];
                for (int row = 0; row < m; row++)
                {
                    int sum = 0;
                    for (int col = 0; col < m; col++)
                    {
                        sum += processMatrix[row, col] * vector[col];
                    }
                    resultVector[row] = MathHelper.Mod(sum, AlphabetSize);
                }

                foreach (int val in resultVector)
                {
                    result.Append((char)(val + 'A'));
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