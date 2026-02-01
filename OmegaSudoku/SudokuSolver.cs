using OmegaSudoku.Exceptions;
using System;
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
            if(Math.Sqrt(Math.Sqrt(board.Length)) % 1 != 0 || board.Length == 1)
            {
                throw new InvalidBoardSize(board);
            }

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
        private int charToInt(char c)
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
                throw new UnsolvableBoard();

            board = new string(solvedBoard);
            timer.Stop();
        }

        private bool SolverRec()
        {
            //fill hidden single

            //fint the index of the best cell that we can check according to the FindBestCell function
            int index = FindBestCell();

            int j = index % size;
            int i = (index - j) / size;

            if (i == -1)
            {
                return true;
            }


            for (int num = 1; num <= size; num++)
            {
                if (IsSafe(i, j, num))
                {
                    //place the number in thee cell
                    solvedBoard[index] = intToChar(num);
                    setBite(i, j, num);

                    //recursive call
                    if (SolverRec())
                    {
                        return true;
                    }

                    //delete the numbeer from the cell
                    solvedBoard[index] = '0';
                    unsetBite(i, j, num);

                }
            }
            //if board is unsolvable - reeturn false
            return false;
        }

        /// <summary>
        /// the function return the cell with the least amount of valuse that can be placed in
        /// </summary>
        private int FindBestCell()
        {
            int bestI = -1;
            int bestJ = -1;
            int minValues = size + 1;         

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
                            return i * size + j;
                        }

                        if (values < minValues)
                        {
                            minValues = values;
                            bestI = i;
                            bestJ = j;
                        }
                    }
                }
            }

            return bestI * size + bestJ;
        }

        private (int r, int c, int n) FillHiddenSingle()
        {
            for(int num = 1; num <= size; num++)
            {
                int mask = 1 << num;


                //rows
                int r = 0, c = 0;
                int counter = 0;
                for(int i =0; i < size; i++)
                {
                    for(int j = 0; j < size; j++)
                    {
                        int options = row[i] | col[j];
                        if((mask & options) == 0)
                        {
                            counter++;
                            r = i;
                            c = j;
                        }
                    }
                }
            }
            return (-1,-1,-1);
        }


        private bool IsHiddenSingles(int i, int j)
        {
            //row
            int options = row[i] | col[j] | box[i / boxSize * boxSize + j / boxSize] | 1;


            for (int k = 0; k < size; k++)
            {
                if(k != j && solvedBoard[i * size + j] == '0')
                {
                    //Console.WriteLine($"\npos: [{i},{k}]");
                    int mask = row[i] | col[k] | box[i / boxSize * boxSize + k / boxSize];
                    mask = (~mask);
                    mask &= 1023;
                    //Console.WriteLine($"op: {Convert.ToString(options,2)}\nma: {Convert.ToString(mask, 2)}");
                    options |= (~mask);
                    options &= 1023;
                }
            }
            if(size - BitOperations.PopCount((uint)options) == 1)
            {
                return true;
            }

            //col
            options = row[i] | col[j] | box[i / boxSize * boxSize + j / boxSize] | 1;
            for (int k = 0; k < size; k++)
            {
                if (k != i && solvedBoard[i * size + j] == '0')
                {
                    //Console.WriteLine($"\npos: [{i},{k}]");
                    int mask = row[i] | col[k] | box[i / boxSize * boxSize + k / boxSize];
                    mask = (~mask);
                    mask &= 1023;
                    //Console.WriteLine($"op: {Convert.ToString(options, 2)}\nma: {Convert.ToString(mask, 2)}");
                    options |= (~mask);
                    options &= 1023;
                }
            }
            if (size - BitOperations.PopCount((uint)options) == 1)
            {
                return true;
            }

            //box
            options = row[i] | col[j] | box[i / boxSize * boxSize + j / boxSize] | 1;
            int index = i * size + j;
            int boxNum = i / boxSize * boxSize + j / boxSize;
            i = (boxNum / boxSize) * boxSize;
            j = (boxNum * boxSize) % size;
            for (int k = 1; k <= size; k++)
            {
                if (i * size + j != index && solvedBoard[i * size + j] == '0')
                {
                    int mask = row[i] | col[j] | box[i / boxSize * boxSize + j / boxSize];
                    mask = (~mask);
                    mask &= 1023;
                    Console.WriteLine($"pos: [{i}, {j}]\nop: {Convert.ToString(options, 2)}\nma: {Convert.ToString(mask, 2)}");
                    options |= (~mask);
                    options &= 1023;
                }
                j++;
                if (k != 0 && k % 3 == 0)
                {
                    i++;
                    j -= boxSize;
                }
            }
            if (size - BitOperations.PopCount((uint)options) == 1)
            {
                return true;
            }


            return false;
        }


        public void getBoxIndex(int num)
        {
            int i = (num / boxSize) * boxSize;
            int j = (num * boxSize) % size;
            for (int k = 1; k <= size; k++)
            {
                Console.Write($"[{i},{j}]");
                j++;
                if(k != 0 && k % 3 == 0)
                {
                    i++;
                    j -= boxSize;
                }
            }


            Console.WriteLine();
        }
    }
}
