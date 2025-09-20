using System.Text;

namespace CryptoCourse.Core.Algorithms.Classical
{
    public static class VigenereCipher
    {
        public static string Process(string text, string key, bool isEncrypt)
        {
            const string EnglishAlphabet = "abcdefghijklmnopqrstuvwxyz";
            const string ArabicAlphabet = "ابتثجحخدذرزسشصضطظعغفقكلمنهوي";

            var result = new StringBuilder();
            int keyIndex = 0;
            key = key.ToLower();

            foreach (char c in text)
            {
                char lowerC = char.ToLower(c);
                int arabicIndex = ArabicAlphabet.IndexOf(lowerC);

                if (arabicIndex != -1) // Arabic character
                {
                    char currentKeyChar = key[keyIndex % key.Length];
                    int keyCharIndex = ArabicAlphabet.IndexOf(currentKeyChar);
                    if (keyCharIndex == -1) { result.Append(c); continue; } // Skip if key char is not Arabic

                    int direction = isEncrypt ? 1 : -1;
                    int newIndex = (arabicIndex + (keyCharIndex * direction)) % ArabicAlphabet.Length;
                    if (newIndex < 0) newIndex += ArabicAlphabet.Length;

                    result.Append(ArabicAlphabet[newIndex]);
                    keyIndex++;
                }
                else if (char.IsLetter(c)) // English character
                {
                    char currentKeyChar = key[keyIndex % key.Length];
                    int keyCharIndex = EnglishAlphabet.IndexOf(currentKeyChar);
                    if (keyCharIndex == -1) { result.Append(c); continue; } // Skip if key char is not English

                    int direction = isEncrypt ? 1 : -1;
                    char offset = char.IsUpper(c) ? 'A' : 'a';
                    int shifted = (c - offset + (keyCharIndex * direction)) % 26;
                    if (shifted < 0) shifted += 26;

                    result.Append((char)(shifted + offset));
                    keyIndex++;
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}