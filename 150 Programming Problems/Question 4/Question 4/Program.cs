//1.4 Write a method to decide if two strings are anagrams or not.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Question_4
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine(anagram(Console.ReadLine(), Console.ReadLine()));
            } while (true);
        }

        static bool anagram(string s1, string s2)
        {
            char[] a1 = s1.ToCharArray();
            char[] a2 = s2.ToCharArray();
            Array.Sort(a1);
            Array.Sort(a2);
            s1 = new string(a1);
            s2 = new string(a2);
            if (String.Compare(s1, s2) == 0)
                return true;
            return false;

        }
    }
}
