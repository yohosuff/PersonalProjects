//1.6 Given an image represented by an NxN matrix, where each pixel in the image is 4 bytes, write a method to rotate the image by 90 degrees. Can you do this in place?

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GDIDrawer;
using System.Drawing;

namespace _6
{
    class Program
    {
        static void Main(string[] args)
        {
            CDrawer canvas = new CDrawer(150,150);
            canvas.Scale = 50;
            

            Color[,] colors = new Color[,] { { Color.Red, Color.Red, Color.Red }, { Color.Green, Color.Green, Color.Green }, { Color.Yellow, Color.Yellow, Color.Yellow } };

            do
            {
                for (int row = 0; row < colors.GetLength(0); ++row)
                    for (int column = 0; column < colors.GetLength(1); ++column)
                        canvas.SetBBScaledPixel(column, row, colors[row, column]);
                Console.ReadKey();
                colors = rotate(colors);

            } while (true);
            


        }

        static Color[,] rotate(Color[,] a)
        {
            Color[,] b = new Color[a.GetLength(0), a.GetLength(1)];

            for (int row = 0, column = a.GetLength(1) - 1; row < a.GetLength(0); ++row, --column)
            {
                for(int i = 0; i < a.GetLength(0); ++i)
                    b[i, column] = a[row, i];
            }

            return b;
        }
    }
}
