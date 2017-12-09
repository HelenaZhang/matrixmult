using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MultMatrix
{
    class RecursiveMultMatrix
    {
        public static int LEAF_SIZE = 32;

        private static Matrix Add(Matrix A, Matrix B)
        {
            Matrix C = new Matrix(A.n);
            for (int i = 0; i < C.n; i++)
            {
                for (int j = 0; j < C.n; j++)
                {
                    C.Mas[i, j] = A.Mas[i, j] + B.Mas[i, j];
                }
            }
            return C;
        }

        private static Matrix Subtract(Matrix A, Matrix B)
        {
            Matrix C = new Matrix (A.n);
            for (int i = 0; i < C.n; i++)
            {
                for (int j = 0; j < C.n; j++)
                {
                    C.Mas[i, j] = A.Mas[i, j] - B.Mas[i, j];
                }
            }
            return C;
        }

        private static int NextPowerOfTwo(int n)
        {
            int log2 = (int)Math.Ceiling(Math.Log(n) / Math.Log(2));
            return (int)Math.Pow(2, log2);
        }

        public static Matrix Strassen(Matrix A, Matrix B)
        {
            // Увеличиваем матрицу, чтобы её размерность была степенью двойки
            int n = A.n;
            int m = NextPowerOfTwo(n);
            Matrix APrep = new Matrix(m);
            Matrix BPrep = new Matrix(m);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    APrep.Mas[i, j] = A.Mas[i, j];
                    BPrep.Mas[i, j] = B.Mas[i, j];
                }
            }

            Matrix CPrep = new Matrix();
            CPrep = StrassenR(APrep, BPrep);
            Matrix C = new Matrix (n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    C.Mas[i, j] = CPrep.Mas[i, j];
                }
            }
            return C;
        }

        private static Matrix StrassenR(Matrix A, Matrix B)
        {
            int n = A.n;

            if (n <= LEAF_SIZE)
            {
                return TrivialMultMatrix.TrivialMult(A, B);
            }
            else
            {
                // По алгоритму матрицы делятся на 4 части
                int newSize = n / 2;
                Matrix a11 = new Matrix(newSize);
                Matrix a12 = new Matrix(newSize);
                Matrix a21 = new Matrix(newSize);
                Matrix a22 = new Matrix(newSize);

                Matrix b11 = new Matrix(newSize);
                Matrix b12 = new Matrix(newSize);
                Matrix b21 = new Matrix(newSize);
                Matrix b22 = new Matrix(newSize);

                Matrix aResult = new Matrix(newSize);
                Matrix bResult = new Matrix(newSize);

                // Разделим матрицы на 4 части
                for (int i = 0; i < newSize; i++)
                {
                    for (int j = 0; j < newSize; j++)
                    {
                        a11.Mas[i, j] = A.Mas[i, j]; // верхняя левая часть
                        a12.Mas[i, j] = A.Mas[i, j + newSize]; // верхняя правая
                        a21.Mas[i, j] = A.Mas[i + newSize, j]; // нижняя левая
                        a22.Mas[i, j] = A.Mas[i + newSize, j + newSize]; // нижняя правая

                        b11.Mas[i, j] = B.Mas[i, j]; // верхняя левая часть
                        b12.Mas[i, j] = B.Mas[i, j + newSize]; // верхняя правая
                        b21.Mas[i, j] = B.Mas[i + newSize, j]; // нижняя левая
                        b22.Mas[i, j] = B.Mas[i + newSize, j + newSize]; // нижняя правая
                    }
                }

                // Вычисление от p1 до p7:
                aResult = Add(a11, a22);
                bResult = Add(b11, b22);
                Matrix p1 = StrassenR(aResult, bResult);
                // p1 = (a11+a22) * (b11+b22)

                aResult = Add(a21, a22); // a21 + a22
                Matrix p2 = new Matrix();
                p2 = StrassenR(aResult, b11);
                // p2 = (a21+a22) * (b11)

                bResult = Subtract(b12, b22); // b12 - b22
                Matrix p3 = new Matrix();
                p3 = StrassenR(a11, bResult);
                // p3 = (a11) * (b12 - b22)

                bResult = Subtract(b21, b11); // b21 - b11
                Matrix p4 = new Matrix();
                p4 = StrassenR(a22, bResult);
                // p4 = (a22) * (b21 - b11)

                aResult = Add(a11, a12); // a11 + a12
                Matrix p5 = new Matrix();
                p5 = StrassenR(aResult, b22);
                // p5 = (a11+a12) * (b22)

                aResult = Subtract(a21, a11); // a21 - a11
                bResult = Add(b11, b12); // b11 + b12
                Matrix p6 = new Matrix();
                p6 = StrassenR(aResult, bResult);
                // p6 = (a21-a11) * (b11+b12)

                aResult = Subtract(a12, a22); // a12 - a22
                bResult = Add(b21, b22); // b21 + b22
                Matrix p7 = new Matrix();
                p7 = StrassenR(aResult, bResult);
                // p7 = (a12-a22) * (b21+b22)

                // Вычисление c21, c21, c11, c22:
                Matrix c12 = new Matrix();
                c12 = Add(p3, p5); // c12 = p3 + p5
                Matrix c21 = new Matrix();
                c21 = Add(p2, p4); // c21 = p2 + p4

                aResult = Add(p1, p4); // p1 + p4
                bResult = Add(aResult, p7); // p1 + p4 + p7
                Matrix c11 = new Matrix();
                c11 = Subtract(bResult, p5);
                // c11 = p1 + p4 - p5 + p7

                aResult = Add(p1, p3); // p1 + p3
                bResult = Add(aResult, p6); // p1 + p3 + p6
                Matrix c22 = new Matrix();
                c22 = Subtract(bResult, p2);
                // c22 = p1 + p3 - p2 + p6

                // Соберем результаты, полученные из частей матриц
                Matrix C = new Matrix(n);
                for (int i = 0; i < newSize; i++)
                {
                    for (int j = 0; j < newSize; j++)
                    {
                        C.Mas[i, j] = c11.Mas[i, j];
                        C.Mas[i, j + newSize] = c12.Mas[i, j];
                        C.Mas[i + newSize, j] = c21.Mas[i, j];
                        C.Mas[i + newSize, j + newSize] = c22.Mas[i, j];
                    }
                }
                return C;
            }
        }
    }
}
