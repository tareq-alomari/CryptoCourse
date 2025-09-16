using System;

namespace CryptoCourse.Utils
{
    public static class MatrixHelper
    {
        // Creates a sub-matrix by removing a given row and column. Helper for Determinant.
        private static int[,] GetSubmatrix(int[,] matrix, int rowToRemove, int colToRemove)
        {
            int size = matrix.GetLength(0);
            int[,] submatrix = new int[size - 1, size - 1];
            int r = 0;
            for (int i = 0; i < size; i++)
            {
                if (i == rowToRemove) continue;
                int c = 0;
                for (int j = 0; j < size; j++)
                {
                    if (j == colToRemove) continue;
                    submatrix[r, c] = matrix[i, j];
                    c++;
                }
                r++;
            }
            return submatrix;
        }

        // Calculates the determinant of a square matrix recursively.
        public static int Determinant(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n != matrix.GetLength(1)) throw new ArgumentException("Matrix must be square.");
            if (n == 1) return matrix[0, 0];
            if (n == 2) return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            int det = 0;
            for (int j = 0; j < n; j++)
            {
                int sign = (j % 2 == 0) ? 1 : -1;
                det += sign * matrix[0, j] * Determinant(GetSubmatrix(matrix, 0, j));
            }
            return det;
        }

        // Transposes a matrix.
        private static int[,] Transpose(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            int[,] transposed = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    transposed[j, i] = matrix[i, j];
                }
            }
            return transposed;
        }

        // Calculates the adjugate of a matrix (transpose of the cofactor matrix).
        private static int[,] Adjugate(int[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n == 1) return new int[,] { { 1 } };

            int[,] cofactorMatrix = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int sign = ((i + j) % 2 == 0) ? 1 : -1;
                    cofactorMatrix[i, j] = sign * Determinant(GetSubmatrix(matrix, i, j));
                }
            }
            return Transpose(cofactorMatrix);
        }

        /// <summary>
        /// Calculates the modular inverse of a square matrix. This is the key to Hill Cipher decryption.
        /// </summary>
        public static int[,] InverseMatrix(int[,] matrix, int modulus)
        {
            int n = matrix.GetLength(0);
            int det = Determinant(matrix);
            int detInverse = MathHelper.ModInverse(MathHelper.Mod(det, modulus), modulus);

            if (detInverse == -1)
            {
                throw new InvalidOperationException("Matrix is not invertible for the given modulus (determinant has no modular inverse).");
            }

            int[,] adjugate = Adjugate(matrix);
            int[,] inverse = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    inverse[i, j] = MathHelper.Mod(adjugate[i, j] * detInverse, modulus);
                }
            }
            return inverse;
        }
    }
}