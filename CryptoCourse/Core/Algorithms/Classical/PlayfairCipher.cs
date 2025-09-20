using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class PlayfairCipher
    {
        // Define the standardized alphabets
        private const string EnglishAlphabet = "abcdefghiklmnopqrstuvwxyz"; // No 'j'
        private const string ArabicAlphabet = "ابتثجحخدذرزسشصضطظعغفقكلمنهوي";

        // A class to hold the context for a given language
        private class PlayfairContext
        {
            public char[,] Matrix { get; }
            public int Rows { get; }
            public int Cols { get; }

            public PlayfairContext(string key, bool isArabic)
            {
                if (isArabic)
                {
                    Rows = 4;
                    Cols = 7;
                    Matrix = GenerateMatrix(key, ArabicAlphabet);
                }
                else
                {
                    Rows = 5;
                    Cols = 5;
                    Matrix = GenerateMatrix(key, EnglishAlphabet);
                }
            }

            private char[,] GenerateMatrix(string key, string alphabet)
            {
                var matrix = new char[Rows, Cols];
                var keyString = new StringBuilder();
                var usedChars = new HashSet<char>();

                // FIX 1: Normalize key and add unique characters
                foreach (char c in NormalizeText(key, IsArabic()))
                {
                    if (usedChars.Add(c))
                    {
                        keyString.Append(c);
                    }
                }

                // Add the rest of the alphabet
                foreach (char c in alphabet)
                {
                    if (usedChars.Add(c))
                    {
                        keyString.Append(c);
                    }
                }

                int index = 0;
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Cols; j++)
                    {
                        matrix[i, j] = keyString[index++];
                    }
                }
                return matrix;
            }

            public (int row, int col) FindPosition(char c)
            {
                for (int i = 0; i < Rows; i++)
                    for (int j = 0; j < Cols; j++)
                        if (Matrix[i, j] == c) return (i, j);
                return (-1, -1);
            }

            public bool IsArabic() => Rows == 4;
        }

        // FIX 2: Create a normalization function for all text
        private static string NormalizeText(string text, bool isArabic)
        {
            var sb = new StringBuilder();
            text = text.ToLower();

            if (isArabic)
            {
                foreach (char c in text)
                {
                    char normalizedChar = c;
                    if ("آأإ".Contains(c)) normalizedChar = 'ا';
                    if (c == 'ة') normalizedChar = 'ت';
                    if (c == 'ى') normalizedChar = 'ي';

                    if (ArabicAlphabet.Contains(normalizedChar))
                    {
                        sb.Append(normalizedChar);
                    }
                }
            }
            else // English
            {
                foreach (char c in text)
                {
                    char normalizedChar = c;
                    if (c == 'j') normalizedChar = 'i';

                    if (EnglishAlphabet.Contains(normalizedChar))
                    {
                        sb.Append(normalizedChar);
                    }
                }
            }
            return sb.ToString();
        }

        private static string PrepareText(string normalizedText, bool isArabic)
        {
            var sb = new StringBuilder(normalizedText);
            char padChar = isArabic ? 'ي' : 'x';

            for (int i = 0; i < sb.Length - 1; i += 2)
            {
                if (sb[i] == sb[i + 1])
                {
                    sb.Insert(i + 1, padChar);
                }
            }

            if (sb.Length % 2 != 0)
            {
                sb.Append(padChar);
            }
            return sb.ToString();
        }

        private static string Process(string text, string key, bool isEncrypt)
        {
            if (string.IsNullOrEmpty(text)) return "";
            bool isArabic = ArabicAlphabet.Contains(text.ToLower()[0]);
            var context = new PlayfairContext(key, isArabic);

            string inputText = isEncrypt ? PrepareText(NormalizeText(text, isArabic), isArabic) : NormalizeText(text, isArabic);
            var result = new StringBuilder();

            for (int i = 0; i < inputText.Length; i += 2)
            {
                var posA = context.FindPosition(inputText[i]);
                var posB = context.FindPosition(inputText[i + 1]);

                if (posA.row == -1 || posB.row == -1) continue; // Should not happen with normalization

                int dir = isEncrypt ? 1 : -1;

                if (posA.row == posB.row) // Same row
                {
                    result.Append(context.Matrix[posA.row, (posA.col + dir + context.Cols) % context.Cols]);
                    result.Append(context.Matrix[posB.row, (posB.col + dir + context.Cols) % context.Cols]);
                }
                else if (posA.col == posB.col) // Same column
                {
                    result.Append(context.Matrix[(posA.row + dir + context.Rows) % context.Rows, posA.col]);
                    result.Append(context.Matrix[(posB.row + dir + context.Rows) % context.Rows, posB.col]);
                }
                else // Rectangle
                {
                    result.Append(context.Matrix[posA.row, posB.col]);
                    result.Append(context.Matrix[posB.row, posA.col]);
                }
            }
            return result.ToString();
        }

        public static string Encrypt(string plaintext, string key) => Process(plaintext, key, true);
        public static string Decrypt(string ciphertext, string key) => Process(ciphertext, key, false);
    }
}