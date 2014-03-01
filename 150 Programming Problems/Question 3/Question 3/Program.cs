//Design an algorithm and write code to remove the duplicate characters in a string without using any additional buffer. NOTE: One or two additional variables are fine. An extra copy of the array is not.
//FOLLOW UP
//Write the test cases for this method.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Question_3
{
    class Program
    {
        static void Main(string[] args)
        {
            string s;
            do
            {
                s = Console.ReadLine();
                Console.WriteLine(removeDuplicates(s));
            } while (true);
        }

        static string removeDuplicates(string s)
        {
            List<char> l = new List<char>();
            StringBuilder sb = new StringBuilder();
            
            foreach(char c in s)
                if (!l.Contains(c))
                {
                    l.Add(c);
                    sb.Append(c);
                }

            return sb.ToString();
            
        }
    }
}
