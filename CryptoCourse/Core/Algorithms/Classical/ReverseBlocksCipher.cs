using System;
using System.Linq;
using System.Text;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class ReverseBlocksCipher
    {
        /// <summary>
        /// Reverses the text in chunks of a specified block size.
        /// Encryption and Decryption are the same operation.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="blockSize">The size of each block to reverse.</param>
        /// <returns>The processed text.</returns>
        public static string Process(string text, int blockSize)
        {
            if (blockSize <= 0)
            {
                throw new ArgumentException("Block size must be a positive integer.", nameof(blockSize));
            }

            var result = new StringBuilder();
            for (int i = 0; i < text.Length; i += blockSize)
            {
                // Get the current block
                string block = text.Substring(i, Math.Min(blockSize, text.Length - i));

                // Reverse the block
                char[] charArray = block.ToCharArray();
                Array.Reverse(charArray);

                // Append the reversed block to the result
                result.Append(new string(charArray));
            }

            return result.ToString();
        }
    }
}