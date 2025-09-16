using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class CaesarCipher
    {
        /// <summary>
        /// Encrypts or decrypts text using the Caesar cipher method.
        /// </summary>
        /// <param name="text">The input text (plaintext or ciphertext).</param>
        /// <param name="key">The integer shift key. Use a negative key for decryption.</param>
        /// <returns>The processed text.</returns>
        public static string Process(string text, int key)
        {
            // A StringBuilder is more efficient for building strings in a loop
            var result = new System.Text.StringBuilder();

            foreach (char c in text)
            {
                if (char.IsLetter(c))
                {
                    // Determine the base character ('A' for uppercase, 'a' for lowercase)
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    // Apply the shift formula
                    int shifted = (c - offset + key) % 26;
                    // Handle negative results for decryption
                    if (shifted < 0)
                    {
                        shifted += 26;
                    }
                    result.Append((char)(shifted + offset));
                }
                else
                {
                    // If the character is not a letter, keep it as is
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}
