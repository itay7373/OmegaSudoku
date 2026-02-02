using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.Exceptions
{
    public class UnsolvableBoardException : Exception
    {
        public UnsolvableBoardException()
        {
            Console.WriteLine("this board is unsolvable.");
        } 
    }
}
