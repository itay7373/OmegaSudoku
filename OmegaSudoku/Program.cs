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
            SudokuSolver s1 = new SudokuSolver("000000000580300000000020100000705000001000400000000090370000005000010200900040000");
            SudokuSolver s2 = new SudokuSolver("000000000000003085001020000000507000004000100090000000500000073002010000000040009");
            Console.WriteLine("First Sudoku board:");
            UI.PrintSudoku("000000000580300000000020100000705000001000400000000090370000005000010200900040000");

            timer1.Start();
            s1.Solve();
            timer1.Stop();
            Console.WriteLine("\nFirst Sudoku board solved:");
            UI.PrintSudoku(s1.getBoard());
            Console.WriteLine("time: " + timer1.Elapsed);

            Console.WriteLine("\nSecond Sudoku board:");
            UI.PrintSudoku(s2.getBoard());
            timer2.Start();
            s2.Solve();
            timer2.Stop();
            Console.WriteLine("\nSecond Sudoku board solved:");
            UI.PrintSudoku(s2.getBoard());
            Console.WriteLine("time: " + timer2.Elapsed);

        }
    }
}
