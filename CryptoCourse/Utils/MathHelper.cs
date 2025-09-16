namespace CryptoCourse.Utils
{
    public static class MathHelper
    {
        /// <summary>
        /// Calculates the Greatest Common Divisor (GCD) of two integers using the Euclidean algorithm.
        /// </summary>
        public static int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Calculates the modular multiplicative inverse of 'a' modulo 'm'.
        /// Returns -1 if no inverse exists.
        /// </summary>
        public static int ModInverse(int a, int m)
        {
            a = a % m;
            for (int x = 1; x < m; x++)
            {
                if ((a * x) % m == 1)
                {
                    return x;
                }
            }
            return -1; // Indicates that no inverse exists
        }

        /// <summary>
        /// A custom modulo operator that handles negative results correctly.
        /// </summary>
        public static int Mod(int n, int m)
        {
            return ((n % m) + m) % m;
        }
    }
}