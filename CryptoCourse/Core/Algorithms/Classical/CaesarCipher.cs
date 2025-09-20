using System.Text;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class CaesarCipher
    {
        public static string Process(string text, int key)
        {
            // Define both alphabets as constants
            const string EnglishAlphabet = "abcdefghijklmnopqrstuvwxyz";
            // As per the textbook, we define the Arabic alphabet
            const string ArabicAlphabet = "ابتثجحخدذرزسشصضطظعغفقكلمنهوي";

            var result = new StringBuilder();

            foreach (char c in text)
            {
                // First, check if the character is Arabic
                int arabicIndex = ArabicAlphabet.IndexOf(char.ToLower(c));

                if (arabicIndex != -1)
                {
                    // It's an Arabic letter, apply the shift using Arabic alphabet size
                    int newIndex = (arabicIndex + key) % ArabicAlphabet.Length;
                    if (newIndex < 0) newIndex += ArabicAlphabet.Length;
                    result.Append(ArabicAlphabet[newIndex]);
                }
                else if (char.IsLetter(c)) // If not Arabic, check if it's an English letter
                {
                    // It's an English letter, use the original logic
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int shifted = (c - offset + key) % 26;
                    if (shifted < 0) shifted += 26;
                    result.Append((char)(shifted + offset));
                }
                else
                {
                    // It's a number, symbol, or space, so keep it as is
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}