using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.Exceptions
{
    public class EmptyBoardException : Exception
    {
        public EmptyBoardException()
        {
            Console.WriteLine("The board must not be empty.");
        }
    }
}
