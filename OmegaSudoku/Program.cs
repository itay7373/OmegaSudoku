using OmegaSudoku.Solver;
using System;
using System.Diagnostics;

namespace OmegaSudoku
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch timer1 = new Stopwatch();
            Stopwatch timer2 = new Stopwatch(); 
            SudokuSolver s1 = new SudokuSolver("000000012000000003002300400001800005060070800000009000500007000000040100003000008");
            SudokuSolver s2 = new SudokuSolver("000000000000003085001020000000507000004000100090000000500000073002010000000040009");


            Console.WriteLine("First Sudoku board:");
            s1.printSudoku();
            timer1.Start();
            s1.Solve();
            timer1.Stop();
            Console.WriteLine("\nFirst Sudoku board solved:");
            s1.printSudoku();
            Console.WriteLine("time: " + timer1.Elapsed);

            Console.WriteLine("\nSecond Sudoku board:");
            s2.printSudoku();
            timer2.Start();
            s2.Solve();
            timer2.Stop();
            Console.WriteLine("\nSecond Sudoku board solved:");
            s2.printSudoku();
            Console.WriteLine("time: " + timer2.Elapsed);

        }
    }
}
