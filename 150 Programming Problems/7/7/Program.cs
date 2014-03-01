//1.7 Write an algorithm such that if an element in an MxN matrix is 0, its entire row and column is set to 0.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7
{
    class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            do
            {
                //int[,] a = new int[random.Next(6, 12), random.Next(10, 40)];
                int[,] a = new int[11, 79];

                for (int row = 0; row < a.GetLength(0); ++row)
                    for (int column = 0; column < a.GetLength(1); ++column)
                        a[row, column] = random.Next(1,10);

                a[random.Next(a.GetLength(0)), random.Next(a.GetLength(1))] = 0;
                a[random.Next(a.GetLength(0)), random.Next(a.GetLength(1))] = 0;


                Console.Clear();
                printArray(a);
                a = zero(a);
                printArray(a);
                Console.ReadKey(true);
            } while (true);
            



        }

        static void printArray(int[,] a)
        {
            for (int row = 0; row < a.GetLength(0); ++row)
            {
                for (int column = 0; column < a.GetLength(1); ++column)
                    Console.Write(a[row, column]);
                Console.WriteLine();
            }
            Console.WriteLine();
                
        }

        static int[,] zero(int[,] a)
        {
            int[,] b = (int[,])a.Clone();

            for(int row = 0; row < a.GetLength(0); ++row)
                for(int column = 0; column < a.GetLength(1); ++column)
                    if (a[row, column] == 0)
                    {
                        for (int i = 0; i < a.GetLength(0); ++i)
                            b[i, column] = 0;
                        for (int i = 0; i < a.GetLength(1); ++i)
                            b[row, i] = 0;
                    }

            return b;
        }
    }
}
