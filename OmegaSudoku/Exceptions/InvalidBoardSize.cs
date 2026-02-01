using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.Exceptions
{
    internal class InvalidBoardSize : Exception
    {
       public InvalidBoardSize(string board)
       {
            Console.WriteLine($"Invalid board length: {board.Length}.");
            Console.WriteLine("The board length must be a perfect square, and its square root must also be a perfect square.");
       }
    }
}
