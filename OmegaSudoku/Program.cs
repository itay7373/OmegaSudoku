using System;
using System.Diagnostics;
using System.Numerics;


namespace OmegaSudoku
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                UI.Start();

                try
                {
                    string board = UI.EnterSudoku();

                    SudokuSolver s = new SudokuSolver(board);

                    UI.PrintSudoku(s.GetBoard());
                    s.Solve();
                    UI.PrintSudoku(s.GetBoard());
                    UI.PrintTime(s.GetTimer());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nplease enter a valid board.");
                }

                UI.End();
            }


            //009032000000700000162000000010020560000900000050000107000000403026009000005870000
            //000060080020000000001000000070000102500030000000000400004201000300700600000000050
            //000000000000003085001020000000507000004000100090000000500000073002010000000040009
            //0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000

            SudokuSolver s1 = new SudokuSolver("000610000020000080004000000010000302600008000000500400005700000000400200100000000");
            SudokuSolver s2 = new SudokuSolver("000060080020000000001000000070000102500030000000000400004201000300700600000000050");



            UI.PrintSudoku(s1.GetBoard());
            s1.Solve();
            UI.PrintSudoku(s1.GetBoard());
            Console.WriteLine("time: " + s1.GetTimer().Elapsed);

            UI.PrintSudoku(s2.GetBoard());
            s2.Solve();
            UI.PrintSudoku(s2.GetBoard());
            Console.WriteLine("time: " + s2.GetTimer().Elapsed);

        }
    }
}
