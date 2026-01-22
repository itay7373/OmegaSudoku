using System;
using System.Numerics;


namespace OmegaSudoku.Solver
{
    public class SudokuSolver
    {
        private char[] solvedBoard;
        private string board;
        private int boxSize;
        private int size;
        private int[] row;
        private int[] col;
        private int[] box;

        public SudokuSolver(string board)
        {
            this.board = board;
            this.solvedBoard = board.ToCharArray();
            this.size = (int)Math.Sqrt(board.Length);
            this.boxSize = (int)Math.Sqrt(this.size);

            this.row = new int[size];
            this.col = new int[size];
            this.box = new int[size];

            

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int val = (int)(board[(i * size) + j] - '0');
                    if (val != 0)
                    {
                        row[i] |= (1 << val);
                        col[j] |= (1 << val);
                        box[(i / boxSize) * boxSize + j / boxSize] |= (1 << val);
                    }
                }
            }
        }

        public string getBoard()
        {
            return this.board;
        }

        public void PrintArrays()
        {
            Console.WriteLine("\nrows:");
            for(int i = 0;i < size; i++)
            {
                Console.Write(Convert.ToString(row[i], 2) + " ");
            }
            Console.WriteLine("\ncol:");
            for (int i = 0; i < size; i++)
            {
                Console.Write(Convert.ToString(col[i], 2) + " ");
            }
            Console.WriteLine("\nbox:");
            for (int i = 0; i < size; i++)
            {
                Console.Write(Convert.ToString(box[i],2) + " ");
            }
            Console.WriteLine();
        }


        public void Solve()
        {
            SolverRec();
            board = new string(solvedBoard);
        }

        private bool SolverRec()
        {
            var index = FindBestCell();

            int i = index.i;
            int j = index.j;

            if (i == -1)
            {
                return true;
            }

            int val = (int)(solvedBoard[(i * size) + j] - '0');
            

            for (int num = 1; num <= size; num++)
            {
                if (IsSafe(i, j, num))
                {
                    solvedBoard[i * size + j] = (char)(num + '0');
                    setBite(i, j, num);

                    if (SolverRec())
                    {
                        return true;
                    }

                    solvedBoard[i * size + j] = '0';
                    unsetBite(i, j, num);

                }
            }
            return false;
        }
        private bool IsSafe(int i, int j, int num)
        {
            return !((row[i] & (1 << num)) != 0 || (col[j] & (1 << num)) != 0 || (box[i / boxSize * boxSize + j / boxSize] & (1 << num)) != 0);
        }

        private void setBite(int i, int j, int num)
        {
            row[i] |= (1 << num);
            col[j] |= (1 << num);
            box[i / boxSize * boxSize + j / boxSize] |= (1 << num);
        }
        private void unsetBite(int i, int j, int num)
        {
            row[i] &= ~(1 << num);
            col[j] &= ~(1 << num);
            box[i / boxSize * boxSize + j / boxSize] &= ~(1 << num);
        }


        private (int i, int j) FindBestCell()
        {
            int bestI = -1;
            int bestJ = -1;
            int minValues = size + 1; 
            

            for(int i = 0; i < size; i++)
            {

                for(int j = 0; j < size; j++)
                {
                    if(solvedBoard[(i * size) + j] == '0')
                    {
                        int values = row[i] | col[j] | box[i / boxSize * boxSize + j / boxSize];
                        values = size - BitOperations.PopCount((uint)values);

                        if (values == 1)
                        {
                            return (i, j);
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

            return (bestI, bestJ);
        }

    }
}
