using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.Exceptions
{
    public class IdenticalNumbersRowException: Exception
    {
        public IdenticalNumbersRowException(int row)
        {
            Console.WriteLine($"Invalid board, Identical numbers in row number {row + 1}.");
        }
    }
}
