//Write code to reverse a C-Style String. (C-String means that “abcd” is represented as five characters, including the null character.)

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
            char[] array;

            do
            {
                Console.Write("Enter a string: ");
                array = StringToCString(Console.ReadLine());
                Console.WriteLine(array);
                Console.WriteLine(reverse(array));
            } while (true);
            
        }

        static char[] reverse(char[] array)
        {
            Stack<char> stack = new Stack<char>();
            List<char> list = new List<char>();

            foreach (char c in array)
                if (c != '\0')
                    stack.Push(c);

            while (stack.Count > 0)
                list.Add(stack.Pop());
            
            list.Add('\0');

            return  list.ToArray();
        }

        static char[] StringToCString(string s)
        {
            char[] array = new char[s.Length + 1];
            array[array.Length - 1] = '\0';
            for (int i = 0; i < s.Length; ++i)
                array[i] = s[i];
            return array;
        }
    }
}
