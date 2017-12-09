using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace MultMatrix
{
    class ThreadTest
    {
        Matrix A = new Matrix();
        Matrix B = new Matrix();
        long m_TrivialAlgTime = 0;
        long m_RecursiveAlgTime = 0;
        public Thread MatrixThread;
        StreamWriter FileOutput;
        int Number;

        public long TrivialAlgTime
        {
            get { return m_TrivialAlgTime; }
        }

        public long RecursiveAlgTime
        {
            get { return m_RecursiveAlgTime; }
        }

        public ThreadTest(int i, int Vi)
        {
            A.InitRandom(Vi);
            B.InitRandom(Vi);
            //Номер теста
            Number = i;
            //Файл результатов для теста
            FileOutput = new StreamWriter("test" + i + ".txt");
            //Запускаем тест в отдельном потоке
            MatrixThread = new Thread(Solve);
            MatrixThread.Start();
        }

        void Solve()
        {
            //Вывод исходных данных
            FileOutput.WriteLine("n = " + A.n + "\n");
            FileOutput.WriteLine("A = ");
            A.Print(FileOutput);
            FileOutput.WriteLine("\nB = ");
            B.Print(FileOutput);
            FileOutput.WriteLine();
            //Расчет с помощью обоих алгоритмов и вывод 
            Program.Solve(A, B, FileOutput, ref m_TrivialAlgTime, ref m_RecursiveAlgTime);
        }
    }
}
