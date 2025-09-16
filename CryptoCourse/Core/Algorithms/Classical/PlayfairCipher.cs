using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class PlayfairCipher
    {
        // تعريف struct بدلاً من tuple
        private struct Position
        {
            public int Row;
            public int Col;
            public Position(int row, int col)
            {
                Row = row;
                Col = col;
            }
        }

        // Helper method to generate the 5x5 key matrix
        private static char[,] GenerateMatrix(string key)
        {
            var matrix = new char[5, 5];
            var keyString = new StringBuilder();
            var usedChars = new HashSet<char>();

            // Sanitize key: uppercase, replace J with I, and add unique chars
            key = key.ToUpper().Replace("J", "I");
            foreach (char c in key)
            {
                if (char.IsLetter(c) && usedChars.Add(c))
                {
                    keyString.Append(c);
                }
            }

            // Add the rest of the alphabet (excluding 'J')
            for (char c = 'A'; c <= 'Z'; c++)
            {
                if (c != 'J' && usedChars.Add(c))
                {
                    keyString.Append(c);
                }
            }

            // Populate the 5x5 matrix
            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matrix[i, j] = keyString[index++];
                }
            }
            return matrix;
        }

        // Helper method to find the row and column of a character
        private static Position FindPosition(char[,] matrix, char c)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (matrix[i, j] == c)
                    {
                        return new Position(i, j);
                    }
                }
            }
            return new Position(-1, -1); // Should not happen with valid input
        }

        // Helper method to prepare the text into digraphs
        private static string PrepareText(string text)
        {
            var sb = new StringBuilder();
            text = text.ToUpper().Replace("J", "I");

            // Remove non-alphabetic characters
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    sb.Append(c);
                }
            }

            // Insert 'X' between duplicate letters in a pair
            for (int i = 0; i < sb.Length - 1; i += 2)
            {
                if (sb[i] == sb[i + 1])
                {
                    sb.Insert(i + 1, 'X');
                }
            }

            // If the length is odd, append an 'X'
            if (sb.Length % 2 != 0)
            {
                sb.Append('X');
            }

            return sb.ToString();
        }

        // Main Encryption Method
        public static string Encrypt(string plaintext, string key)
        {
            var matrix = GenerateMatrix(key);
            var preparedText = PrepareText(plaintext);
            var ciphertext = new StringBuilder();

            for (int i = 0; i < preparedText.Length; i += 2)
            {
                char a = preparedText[i];
                char b = preparedText[i + 1];
                var posA = FindPosition(matrix, a);
                var posB = FindPosition(matrix, b);

                if (posA.Row == posB.Row) // Same row
                {
                    ciphertext.Append(matrix[posA.Row, (posA.Col + 1) % 5]);
                    ciphertext.Append(matrix[posB.Row, (posB.Col + 1) % 5]);
                }
                else if (posA.Col == posB.Col) // Same column
                {
                    ciphertext.Append(matrix[(posA.Row + 1) % 5, posA.Col]);
                    ciphertext.Append(matrix[(posB.Row + 1) % 5, posB.Col]);
                }
                else // Rectangle
                {
                    ciphertext.Append(matrix[posA.Row, posB.Col]);
                    ciphertext.Append(matrix[posB.Row, posA.Col]);
                }
            }
            return ciphertext.ToString();
        }

        // Main Decryption Method
        public static string Decrypt(string ciphertext, string key)
        {
            var matrix = GenerateMatrix(key);
            var plaintext = new StringBuilder();
            ciphertext = ciphertext.ToUpper().Replace("J", "I");

            for (int i = 0; i < ciphertext.Length; i += 2)
            {
                char a = ciphertext[i];
                char b = ciphertext[i + 1];
                var posA = FindPosition(matrix, a);
                var posB = FindPosition(matrix, b);

                if (posA.Row == posB.Row) // Same row
                {
                    plaintext.Append(matrix[posA.Row, (posA.Col + 4) % 5]); // +4 is equivalent to -1
                    plaintext.Append(matrix[posB.Row, (posB.Col + 4) % 5]);
                }
                else if (posA.Col == posB.Col) // Same column
                {
                    plaintext.Append(matrix[(posA.Row + 4) % 5, posA.Col]);
                    plaintext.Append(matrix[(posB.Row + 4) % 5, posB.Col]);
                }
                else // Rectangle
                {
                    plaintext.Append(matrix[posA.Row, posB.Col]);
                    plaintext.Append(matrix[posB.Row, posA.Col]);
                }
            }
            return plaintext.ToString();
        }
    }
}
