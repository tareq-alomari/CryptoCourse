using System.Text;
using System.Linq;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class RailFenceCipher
    {
        public static string Encrypt(string plainText, int key)
        {
            if (key <= 1)
            {
                return plainText;
            }

            // Create an array of StringBuilders, one for each rail
            var rails = new StringBuilder[key];
            for (int i = 0; i < key; i++)
            {
                rails[i] = new StringBuilder();
            }

            int currentRow = 0;
            bool goingDown = true;

            // Zigzag through the rails
            foreach (char c in plainText)
            {
                rails[currentRow].Append(c);

                if (currentRow == 0)
                {
                    goingDown = true;
                }
                else if (currentRow == key - 1)
                {
                    goingDown = false;
                }

                currentRow += goingDown ? 1 : -1;
            }

            // Concatenate the rails to get the ciphertext
            var result = new StringBuilder();
            foreach (var rail in rails)
            {
                result.Append(rail);
            }

            return result.ToString();
        }

        public static string Decrypt(string cipherText, int key)
        {
            if (key <= 1)
            {
                return cipherText;
            }

            // Create the fence as a 2D array of characters
            char[,] fence = new char[key, cipherText.Length];
            int currentRow = 0;
            bool goingDown = true;

            // 1. Mark the positions in the fence with a placeholder
            for (int j = 0; j < cipherText.Length; j++)
            {
                fence[currentRow, j] = '*'; // Placeholder

                if (currentRow == 0) goingDown = true;
                else if (currentRow == key - 1) goingDown = false;

                currentRow += goingDown ? 1 : -1;
            }

            // 2. Fill the fence with the ciphertext characters row by row
            int index = 0;
            for (int i = 0; i < key; i++)
            {
                for (int j = 0; j < cipherText.Length; j++)
                {
                    if (fence[i, j] == '*' && index < cipherText.Length)
                    {
                        fence[i, j] = cipherText[index++];
                    }
                }
            }

            // 3. Read the fence in zigzag order to get the plaintext
            var result = new StringBuilder();
            currentRow = 0;
            goingDown = true;
            for (int j = 0; j < cipherText.Length; j++)
            {
                result.Append(fence[currentRow, j]);

                if (currentRow == 0) goingDown = true;
                else if (currentRow == key - 1) goingDown = false;

                currentRow += goingDown ? 1 : -1;
            }

            return result.ToString();
        }
    }
}