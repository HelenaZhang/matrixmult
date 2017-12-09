using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MultMatrix
{
    class Matrix
    {
        public double[,] Mas;
        public int n;

        public void InitFromFile(StreamReader FileInput)
        {
            for (int i = 0; i < n; i++)
            {
                string[] MatrixStr = FileInput.ReadLine().Split(' ');
                for (int j = 0; j < n; j++)
                {
                    Mas[i, j] = int.Parse(MatrixStr[j]);
                }
            }
        }

        public void InitRandom(int Size)
        {
            Matrix Tmp = new Matrix();
            TestGeneration.GenerMatrix(Size, ref Tmp);
            n = Size;
            Mas = Tmp.Mas;
        }

        public void Print(StreamWriter FileOutput)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    FileOutput.Write(Mas[i, j] + " ");
                }
                FileOutput.WriteLine();
            }
        }

        public Matrix()
        {

        }

        public Matrix(int Size)
        {
            n = Size;
            Mas = new double[n, n];
        }
    }
}
