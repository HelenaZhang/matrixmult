using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace MultMatrix
{
    class Program
    {
        //Решение, подсчет времени и вывод в файл, для разных тестов один метод
        public static void Solve(Matrix A, Matrix B, StreamWriter FileOutput, ref long TimeTrivial, ref long TimeRec)
        {
            Matrix Result = new Matrix();

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Result = TrivialMultMatrix.TrivialMult(A, B);

            stopWatch.Stop();
            TimeTrivial = stopWatch.ElapsedMilliseconds;

            FileOutput.WriteLine("Trivial algorithm: \n");
            Result.Print(FileOutput);
            FileOutput.WriteLine("Time = " + TimeTrivial);

            FileOutput.WriteLine("\nRecursive algorithm: \n");

            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();

            Result = RecursiveMultMatrix.Strassen(A, B);

            stopWatch1.Stop();
            TimeRec = stopWatch1.ElapsedMilliseconds;

            Result.Print(FileOutput);
            FileOutput.WriteLine("Time = " + TimeRec);

            FileOutput.Close();
        }

        //Т.к. многопоточность, проверяем, что все потоки выполнены, прежде чем считать среднее время
        static bool IsThreadsAlive(List<ThreadTest> TestList)
        {
            bool IsThreadAlive = false;
            foreach(ThreadTest test in TestList)
            {
                if (test.MatrixThread.IsAlive)
                    return true;
            }
            return IsThreadAlive;
        }

        //Для работы с несколькими сгенерированными тестами
        static void WorkWithTestGeneration()
        {
            Console.WriteLine("Введите размерность: ");
            int Vi = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите количество тестов: ");
            int NumberTest = Convert.ToInt32(Console.ReadLine());
            List<ThreadTest> TestList = new List<ThreadTest>();

            for (int i = 0; i < NumberTest; i++)
            {
                TestList.Add(new ThreadTest(i, Vi));
            }

            while (IsThreadsAlive(TestList));

            long SumTrivial = 0;
            long SumRec = 0;
            foreach(ThreadTest test in TestList)
            {
                SumTrivial += test.TrivialAlgTime;
                SumRec += test.RecursiveAlgTime;
            }
            Console.WriteLine("Trivial algorithm time = " + Convert.ToDouble(SumTrivial) / TestList.Count
                    + "\n" + "Recursive algorithm time = " + Convert.ToDouble(SumRec) / TestList.Count + "\n\n");
        }

        //Худший вариант - Vi = Vmax
        static void WorkWithMaxValue()
        {
            StreamWriter FileOutput = new StreamWriter("output.txt");
            Matrix A = new Matrix();
            Matrix B = new Matrix();
            long TimeTrivial = 0;
            long TimeRec = 0;

            A.InitRandom(TestGeneration.Vmax);
            B.InitRandom(TestGeneration.Vmax);

            Solve(A, B, FileOutput, ref TimeTrivial, ref TimeRec);
            Console.WriteLine("Trivial algorithm time = " + TimeTrivial
                                + "\n" + "Recursive algorithm time = " + TimeRec + "\n\n");
        }

        //Для чтения из файла
        static void WorkWithFile()
        {
            StreamReader FileInput = new StreamReader("input.txt");
            StreamWriter FileOutput = new StreamWriter("output.txt");
            long TimeTrivial = 0;
            long TimeRec = 0;
            string Size = FileInput.ReadLine();

            Matrix A = new Matrix(Convert.ToInt32(Size));
            Matrix B = new Matrix(Convert.ToInt32(Size));

            A.InitFromFile(FileInput);
            B.InitFromFile(FileInput);

            Solve(A, B, FileOutput, ref TimeTrivial, ref TimeRec);
            Console.WriteLine("Trivial algorithm time = " + TimeTrivial
                                 + "\n" + "Recursive algorithm time = " + TimeRec + "\n\n");
        }

        static void Main(string[] args)
        {
            string TestType;
            do
            {
                Console.WriteLine("Выберите тест: \n1. Из файла\n2. Несколько сгенерированных тестов\n3. Тест с максимальным значением\n0. Выход");
                TestType = Console.ReadLine();
                if (TestType != "0")
                {
                    Console.WriteLine("Введите Leaf size: ");
                    RecursiveMultMatrix.LEAF_SIZE = Convert.ToInt32(Console.ReadLine());
                }

                switch (TestType)
                {
                    case "1":
                        {
                            WorkWithFile();
                            Console.WriteLine("Расчет окончен, результат в файле output.txt");
                        }
                        break;
                    case "2":
                        {
                            WorkWithTestGeneration();
                        }
                        break;
                    case "3":
                        {
                            WorkWithMaxValue();
                            Console.WriteLine("Расчет окончен, результат в файле output.txt");
                        }
                        break;
                    case "0": break;
                    default:
                        {
                            Console.WriteLine("Ошибка ввода! Повторите, пожалуйста, выбор.");
                        }
                        break;
                }
            } while (TestType != "0");
        }
    }
}
