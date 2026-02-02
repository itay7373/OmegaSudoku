using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.Exceptions
{
    public class InvalidBoardSizeException : Exception
    {
       public InvalidBoardSizeException(string board)
       {
            Console.WriteLine($"Invalid board length: {board.Length}.");
            Console.WriteLine("The board length must be a perfect square, and its square root must also be a perfect square.");
       }
    }
}
