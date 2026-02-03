using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.Exceptions
{
    public class IdenticalNumbersColumnException : Exception
    {
        public IdenticalNumbersColumnException(int col)
        {
            Console.WriteLine($"Invalid board, Identical numbers in column number {col + 1}.");
        }
    }
}
