using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.Exceptions
{
    public class InvalidValusException : Exception
    {
        public InvalidValusException(string board, int index)
        {
            Console.WriteLine($"the value {board[index]} in index {index} is invalid \npleaese enter a valid values only.");
        }
    }
}
