using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MultMatrix
{
    class TrivialMultMatrix
    {
        public static Matrix TrivialMult (Matrix A, Matrix B)
        {
            Matrix C = new Matrix(A.n);
            for (int i = 0; i < A.n; i++)
            {
                for (int k = 0; k < A.n; k++)
                {
                    C.Mas[i, k] = 0;
                    for (int j = 0; j < A.n; j++)
                    {
                        C.Mas[i, k] += A.Mas[i, j] * B.Mas[j, k];
                    }
                }
            }
            return C;
        }
    }
}
