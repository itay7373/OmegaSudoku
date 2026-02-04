using OmegaSudoku.BoardVerify;
using OmegaSudoku.Exceptions;
using OmegaSudoku.IO;
using System;
using System.Collections;
using System.Diagnostics;
using System.Numerics;


namespace OmegaSudoku.Solver
{
    public class SudokuSolver
    {
        private string board;
        private char[] solvedBoard;
        private readonly int boxSize;
        private readonly int size;
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
                    int val = CharToInt(board[(i * size) + j]);
                    if (val != 0)
                    {
                        ///check if the board is valid
                        ///throw exception if the value already exist in the same row, colunm or box
                        int validationMask = (1 << val);
                        if ((row[i] & validationMask) != 0)
                        {
                            UI.PrintSudoku(board);
                            throw new IdenticalNumbersRowException(i);
                        }
                        if((col[j] & validationMask) != 0)
                        {
                            UI.PrintSudoku(board);
                            throw new IdenticalNumbersColumnException(j);
                        }
                        if((box[(i / boxSize) * boxSize + j / boxSize] & validationMask) != 0)
                        {
                            UI.PrintSudoku(board);
                            throw new IdenticalNumbersBoxException(i, j, boxSize);
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
        private void SetBit(int r, int c, int num)
        {
            row[r] |= (1 << num);
            col[c] |= (1 << num);
            box[r / boxSize * boxSize + c / boxSize] |= (1 << num);
        }
        private void UnsetBit(int r, int c, int num)
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
        public static int CharToInt(char c)
        {
            if (c >= '0' && c <= '9') return c - '0'; 
            if (c >= 'A' && c <= 'Z') return c - 'A' + 10;

            return -1; 
        }
        private char IntToChar(int n)
        {
            if (n >= 0 && n <= 9) return (char)(n + '0');
            else if (n >= 10 && n <= 25) return (char)(n - 10 + 'A');
            else return '?';
        }

        /// <summary>
        /// place the number in thee cell
        /// </summary>
        private void SetCell(int r, int c, int num)
        {
            solvedBoard[r * size + c] = IntToChar(num);
            SetBit(r, c, num);
        }
        /// <summary>
        /// delete the number from the cell
        /// </summary>
        private void UnsetCell(int r, int c, int num)
        {
            solvedBoard[r * size + c] = '0';
            UnsetBit(r, c, num);
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
            var hs = FindHiddenSingle();
            if (hs.num > 0)
            {
                SetCell(hs.r, hs.c, hs.num);

                if (SolverRec())
                {
                    return true;
                }

                UnsetCell(hs.r, hs.c, hs.num);
                return false;
            }

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
                    SetCell(i, j, num);

                    //recursive call
                    if (SolverRec())
                    {
                        return true;
                    }

                    UnsetCell(i, j, num);

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

                        //calculate how many empty neighbors the cell affect
                        int impact = 3 * size - BitOperations.PopCount((uint)row[i]) 
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

        /// <summary>
        /// find numbers with "hidden single" technique
        /// </summary>
        private (int r, int c, int num) FindHiddenSingle()
        {
            int counter = 0;
            int lastR = -1;
            int lastC = -1;
            //go throw all the numbers
            for (int num = 1; num <= size; num++)
            {
                int mask = (1 << num);
                for (int i = 0; i < size; i++)
                {
                    //check the rows
                    //check if the number is not already placed in the row
                    if ((row[i] & mask) == 0)
                    {
                        counter = 0;
                        for (int c = 0; c < size; c++)
                        {
                            //if the number can be placed in the cell, increase the counter
                            if (solvedBoard[i * size + c] == '0' && IsSafe(i, c, num))
                            {
                                lastR = i;
                                lastC = c;
                                counter++;
                            }
                        }
                        //if the counter is one, thee function found a hidden single
                        if (counter == 1)
                        {
                            //return the position and the number
                            return (lastR, lastC, num);
                        }
                    }


                    //check the cols - same as rows
                    if ((col[i] & mask) == 0)
                    {
                        counter = 0;
                        for (int r = 0; r < size; r++)
                        {
                            if (solvedBoard[r * size + i] == '0' && IsSafe(r, i, num))
                            {
                                lastR = r;
                                lastC = i;
                                counter++;
                            }
                        }
                        if (counter == 1)
                        {
                            return (lastR, lastC, num);
                        }
                    }

                    //check the boxes, almost as the rows and cols
                    if ((box[i] & mask) == 0)
                    {
                        counter = 0;
                        //find the start index of each box in the board (the top right cell in each box) 
                        int boxStartIndex = (i * boxSize + (i / boxSize) * boxSize / size + (i / boxSize) * size * 2);
                        int boxCStartIndex = boxStartIndex % size;
                        int boxRStrartIndex = boxStartIndex / size;
                        for (int r = 0; r < boxSize; r++)
                        {
                            for (int c = 0; c < boxSize; c++)
                            {
                                if (solvedBoard[(boxRStrartIndex + r) * size + boxCStartIndex + c] == '0' && IsSafe(boxRStrartIndex + r, boxCStartIndex + c, num))
                                {
                                    lastR = boxRStrartIndex + r;
                                    lastC = boxCStartIndex + c;
                                    counter++;
                                }
                            }
                        }
                    }
                }
            }
            return (-1, -1, -1);
        }
    }
}
