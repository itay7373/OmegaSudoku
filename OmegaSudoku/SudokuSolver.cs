using OmegaSudoku.Exceptions;
using System;
using System.Collections;
using System.Diagnostics;
using System.Numerics;


namespace OmegaSudoku
{
    public class SudokuSolver
    {
        private string board;
        private char[] solvedBoard;
        private int boxSize;
        private int size;
        private int[] row;
        private int[] col;
        private int[] box;
        private Stopwatch timer; 

        public SudokuSolver(string board)
        {
            BoardVerifier.VerifieBoard(board);

            this.board = board;
            this.solvedBoard = board.ToCharArray();
            this.size = (int)Math.Sqrt(board.Length);
            this.boxSize = (int)Math.Sqrt(this.size);

            this.row = new int[size];
            this.col = new int[size];
            this.box = new int[size];

            this.timer = new Stopwatch();   

            //fill the bitmask arrays with all the values
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int val = charToInt(board[(i * size) + j]);
                    if (val != 0)
                    {
                        ///check if the board is valid
                        ///throw exception if the value already exist in the same row, colunm or box
                        int validationMask = (1 << val);
                        if ((row[i] & validationMask) != 0 || (col[j] & validationMask) != 0 | (box[(i / boxSize) * boxSize + j / boxSize] & validationMask) != 0)
                        {
                            throw new UnsolvableBoardException();
                        }

                        row[i] |= (1 << val);
                        col[j] |= (1 << val);
                        box[(i / boxSize) * boxSize + j / boxSize] |= (1 << val);
                    }
                }
            }
        }

        public Stopwatch GetTimer()
        {
            return this.timer;
        }
        public string GetBoard()
        {
            return this.board;
        }

        /// <summary>
        /// this functions get a position of a cell and a number
        /// th functions check if the number can be placed in that cell 
        /// 
        /// return true if the number can be plased in the cell, else return false
        /// </summary>
        private bool IsSafe(int r, int c, int num)
        {
            return !((row[r] & (1 << num)) != 0 || (col[c] & (1 << num)) != 0 || (box[r / boxSize * boxSize + c / boxSize] & (1 << num)) != 0);
        }


        /// <summary>
        /// the functions setBite and unsetBite a position of a cell and a number
        /// they place the number in the cell in thee bitmask arrays (row, col and box arrays)
        /// </summary>
        private void setBite(int r, int c, int num)
        {
            row[r] |= (1 << num);
            col[c] |= (1 << num);
            box[r / boxSize * boxSize + c / boxSize] |= (1 << num);
        }
        private void unsetBite(int r, int c, int num)
        {
            row[r] &= ~(1 << num);
            col[c] &= ~(1 << num);
            box[r / boxSize * boxSize + c / boxSize] &= ~(1 << num);
        }

        /// <summary>
        /// the function intToChar get an int and returns a char, the function charToInt get a char and returns an int
        /// if the int is above 10 it returns a uppersent letter where A repreesnt 10, B - 11 and so on.. 
        /// the char to int is doing the same thing reversed. 
        /// </summary>
        public static int charToInt(char c)
        {
            if (c >= '0' && c <= '9') return c - '0'; 
            if (c >= 'A' && c <= 'Z') return c - 'A' + 10;

            return -1; 
        }
        private char intToChar(int n)
        {
            if (n >= 0 && n <= 9) return (char)(n + '0');
            else if (n >= 10 && n <= 25) return (char)(n - 10 + 'A');
            else return '?';
        }

        /// <summary>
        /// this functions start the timer, call the SolverRec (who solve the board), change the unsolved board to the soalved board and stops the timer
        /// </summary>
        public void Solve()
        {
            timer.Start();
            if (!SolverRec())
                throw new UnsolvableBoardException();

            board = new string(solvedBoard);
            timer.Stop();
        }

        private bool SolverRec()
        {
            //find the index of the best cell that we can check according to the FindBestCell function
            var index = FindBestCell();

            int i = index.i;
            int j = index.j;

            if (i == -1)
            {
                return true;
            }


            for (int num = 1; num <= size; num++)
            {
                if (IsSafe(i, j, num))
                {
                    //place the number in thee cell
                    solvedBoard[i * size  + j] = intToChar(num);
                    setBite(i, j, num);

                    //recursive call
                    if (SolverRec())
                    {
                        return true;
                    }

                    //delete the numbeer from the cell
                    solvedBoard[i * size + j] = '0';
                    unsetBite(i, j, num);

                }
            }
            //if board is unsolvable - reeturn false
            return false;
        }

        /// <summary>
        /// the function return the cell with the least amount of valuse that can be placed in
        /// </summary>
        private (int i, int j) FindBestCell()
        {
            int bestI = -1;
            int bestJ = -1;
            int minValues = size + 1;
            int maxImpact = -1; 

            for(int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    //check if the cell is empty
                    if(solvedBoard[(i * size) + j] == '0')
                    {
                        //check how many values can bee placed in that cell
                        int values = row[i] | col[j] | box[i / boxSize * boxSize + j / boxSize];
                        values = size - BitOperations.PopCount((uint)values);

                        //if there is only one value that can b in this cell, the function return this cell immediately
                        if (values == 1)
                        {
                            return (i, j);
                        }

                        int impact = 3 * size
                            - BitOperations.PopCount((uint)row[i]) 
                            - BitOperations.PopCount((uint)col[j]) 
                            - BitOperations.PopCount((uint)box[i / boxSize * boxSize + j / boxSize]);

                        if ((values < minValues) || (impact > maxImpact && values == minValues))
                        {
                            maxImpact = impact;
                            minValues = values;
                            bestI = i;
                            bestJ = j;
                        }
                    }
                }
            }

            return (bestI, bestJ);
        }
    }
}
