//Implement an algorithm to determine if a string has all unique characters. What if you can not use additional data structures?

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Question_1
{
    class Program
    {
        static void Main(string[] args)
        {
            string s;

            do
            {
                Console.Write("Enter a string: ");
                s = Console.ReadLine();

                if (uniqueNoDataStructures(s))
                    Console.WriteLine("This string has all unique characters.");
                else
                    Console.WriteLine("Some characters in this string are the same.");
            } while (true);

        }

        static bool uniqueNoDataStructures(string s)
        {
            for (int i = 0; i < s.Length; ++i)
            {
                for (int j = i + 1; j < s.Length; ++j)
                {
                    if (s[i] == s[j])
                        return false;
                }
            }
            return true;
        }

        static bool unique(string s)
        {
            List<char> letters = new List<char>();

            foreach (char c in s)
            {
                if (letters.Contains(c))
                    return false;
                else
                    letters.Add(c);
            }

            return true;
        }
    }
}
