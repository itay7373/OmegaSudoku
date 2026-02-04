using OmegaSudoku.IO;
using OmegaSudoku.Solver;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
                catch (Exception e)
                {
                    UI.ExceptionCaught();
                }

                UI.End();
            }

            //009032000000700000162000000010020560000900000050000107000000403026009000005870000
            //000060080020000000001000000070000102500030000000000400004201000300700600000000050
            //000000000000003085001020000000507000004000100090000000500000073002010000000040009

        }
    }
}
