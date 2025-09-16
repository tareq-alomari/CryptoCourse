using System;
using System.Linq;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class ReverseTextCipher
    {
        /// <summary>
        /// Reverses the given text. Encryption and Decryption are the same operation.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <returns>The reversed text.</returns>
        public static string Process(string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
    }
}