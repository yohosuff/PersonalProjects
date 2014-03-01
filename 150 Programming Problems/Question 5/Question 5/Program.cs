//1.5 Write a method to replace all spaces in a string with ‘%20’.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Question_5
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine(replace(Console.ReadLine()));
            } while (true);
        }

        static string replace(string s)
        {
            return s.Replace(" ", "%20");
        }
    }
}
