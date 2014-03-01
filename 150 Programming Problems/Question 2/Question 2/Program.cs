using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Question_2
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Write("Enter a string to reverse: ");
                string s = Console.ReadLine();
                Console.WriteLine(reverse(s));
            } while (true);
            
        }

        static string reverse(string s)
        {
            Stack<char> stack = new Stack<char>();
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
                stack.Push(c);
            while (stack.Count != 0)
                sb.Append(stack.Pop());
            return sb.ToString();
        }
    }
}
