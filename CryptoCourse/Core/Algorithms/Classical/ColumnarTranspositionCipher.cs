using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class ColumnarTranspositionCipher
    {
        public static string Encrypt(string plainText, string key)
        {
            // Get the order of columns based on the alphabetical order of the key
            var columnOrder = key.Select((c, i) => new { Char = c, Index = i })
                                 .OrderBy(k => k.Char)
                                 .Select(k => k.Index)
                                 .ToArray();

            int numCols = key.Length;
            int numRows = (int)Math.Ceiling((double)plainText.Length / numCols);
            char[,] grid = new char[numRows, numCols];

            // Fill the grid row by row
            int index = 0;
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    grid[r, c] = (index < plainText.Length) ? plainText[index++] : 'X'; // Pad with 'X'
                }
            }

            // Read the grid column by column based on the key order
            var ciphertext = new StringBuilder();
            foreach (int c in columnOrder)
            {
                for (int r = 0; r < numRows; r++)
                {
                    ciphertext.Append(grid[r, c]);
                }
            }
            return ciphertext.ToString();
        }

        public static string Decrypt(string cipherText, string key)
        {
            var sortedKey = key.Select((c, i) => new { Char = c, Index = i })
                               .OrderBy(k => k.Char)
                               .ToArray();

            int numCols = key.Length;
            int numRows = (int)Math.Ceiling((double)cipherText.Length / numCols);
            char[,] grid = new char[numRows, numCols];

            // Determine the order of columns to write into
            int[] writeOrder = new int[numCols];
            for (int i = 0; i < numCols; i++)
            {
                writeOrder[sortedKey[i].Index] = i;
            }

            // Fill the grid column by column
            int index = 0;
            foreach (int originalIndex in sortedKey.Select(k => k.Index))
            {
                for (int r = 0; r < numRows; r++)
                {
                    if (index < cipherText.Length)
                    {
                        grid[r, originalIndex] = cipherText[index++];
                    }
                }
            }

            // Read the grid row by row to get the plaintext
            var plaintext = new StringBuilder();
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    plaintext.Append(grid[r, c]);
                }
            }
            return plaintext.ToString().TrimEnd('X');
        }
    }
}