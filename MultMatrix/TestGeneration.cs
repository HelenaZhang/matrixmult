using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MultMatrix
{
    class TestGeneration
    {
        public const int Vmax = 1024;
        const int Vmin = 0;

        public static void GenerMatrix(int Vi, ref Matrix RandomMatrix)
        {
            Random MatrixRandom = new Random();
            RandomMatrix = new Matrix(Vi);

            for (int i = 0; i < RandomMatrix.n; i++)
            {
                for (int j = 0; j < RandomMatrix.n; j++)
                {
                    RandomMatrix.Mas[i, j] = MatrixRandom.Next(1, 100);
                }
            }
        }
    }
}
