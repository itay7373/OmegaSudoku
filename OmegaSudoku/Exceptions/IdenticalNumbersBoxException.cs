using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.Exceptions
{
    public class IdenticalNumbersBoxException: Exception
    {
        public IdenticalNumbersBoxException(int row, int col, int boxSize)
        {
            Console.WriteLine($"Invalid board, Identical numbers in box number {(row / boxSize) * boxSize + col / boxSize}.");
        }
    }
}
