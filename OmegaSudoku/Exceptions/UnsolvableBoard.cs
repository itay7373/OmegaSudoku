using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.Exceptions
{
    internal class UnsolvableBoard : Exception
    {
        public UnsolvableBoard()
        {
            Console.WriteLine("this board is unsolvable.");
        } 
    }
}
