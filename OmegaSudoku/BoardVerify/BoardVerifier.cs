using OmegaSudoku.Exceptions;
using OmegaSudoku.Solver;
using System;
using System.Collections.Generic;
using System.Text;

namespace OmegaSudoku.BoardVerify
{
    internal class BoardVerifier
    {
        public static void VerifieBoard(string board)
        {
            if(board.Length == 0)
            {
                throw new EmptyBoardException();
            }
                
            //check if the board lentgh is valid 
            if (Math.Sqrt(Math.Sqrt(board.Length)) % 1 != 0 || board.Length == 1)
            {
                throw new InvalidBoardSizeException(board);
            }
            //check if all the valuese are valid (numbers or letters, depends on the board size. 
            for(int i =0; i < board.Length; i++)
            {
                int value = SudokuSolver.charToInt(board[i]);
                if (value < 0 || value > Math.Sqrt(board.Length))
                {
                    throw new InvalidValusException(board, i);
                }
            }
        }
    }
}
