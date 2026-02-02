using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace OmegaSudoku
{
    internal class UI
    {
        public static void Start()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------------");

        }

        public static string EnterSudoku()
        {
            Console.WriteLine("please enter your sudoku board: ");
            return Console.ReadLine();
        }

        public static void PrintTime(Stopwatch s)
        {
            Console.WriteLine("time: " + s.Elapsed);
        }
        public static void End()
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("please press ENTER when you want to solve another board.");
            Console.ReadLine();
        }


        public static void PrintSudoku(string board)
        {
            int size = (int)Math.Sqrt(board.Length);
            int boxSize = (int)Math.Sqrt(size);

            Console.WriteLine(new string('-', 2 * size + 2 * boxSize + 1));
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (j % boxSize == 0)
                    {
                        Console.Write("| ");
                    }
                    Console.Write(board[i * size + j] == '0' ? " " : board[i * size + j].ToString());
                    Console.Write(" ");
                }
                Console.WriteLine("|");

                if ((i + 1) % boxSize == 0)
                {
                    Console.WriteLine(new string('-', 2 * size + 2 * boxSize + 1));
                }
            }
        }
    }
}
