//1.8 Assume you have a method isSubstring which checks if one word is a substring of another. Given two strings, s1 and s2,
//write code to check if s2 is a rotation of s1 using only one call to isSubstring (i.e., “waterbottle” is a rotation of “erbottlewat”).

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _8
{
    class Program
    {
        static void Main(string[] args)
        {

            do
            {
                Console.WriteLine(rotation(Console.ReadLine(), Console.ReadLine()));
                
            } while (true);
            
        }

        static bool rotation(string a, string b)
        {
            string c = a + a + a;
            if(c.Contains(b))
                return true;
            return false;
        }
    }
}
