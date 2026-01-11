using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OmegaSudoku.Solver
{
    internal class SudokuSolver
    {
        char[] solvedSudoku;
        string sudoku;
        int size;
        int[] row;
        int[] col;
        int[] box;

        public SudokuSolver(string sudoku)
        {
            this.sudoku = sudoku;
            this.solvedSudoku = sudoku.ToCharArray();
            this.size = (int)Math.Sqrt(sudoku.Length);

            this.row = new int[size];
            this.col = new int[size];
            this.box = new int[size];

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int val = (int)(sudoku[(i * size) + j] - '0');
                    if (sudoku[(i * size) + j]!= '0')
                    {
                        row[i] |= (1 << val);
                        col[j] |= (1 << val);
                        box[(i / 3) * 3 + j / 3] |= (1 << val);
                    }
                }
            }
        }

        private bool IsSafe(int i , int j, int num)
        {
            if ((row[i] & (1 << num)) != 0 || (col[j] & (1<<num)) != 0 || (box[i/3 * 3 + j /3] &(1<<num)) != 0)
            {
                return false;
            }
            return true;
        }

        private bool SolverRec(int i = 0 , int j = 0)
        {
            if(i == size - 1 && j == size)
            {
                return true;
            }

            if(j == size)
            {
                i++;
                j = 0;
            }

            int val = (int)(solvedSudoku[(i * size) + j] - '0');
    
            if(val != 0)
            {
                return SolverRec(i, j + 1);
            }

            for(int num = 1; num <= size; num++)
            {
                if(IsSafe(i, j, num))
                {
                    solvedSudoku[i * size + j] = (char)(num + '0');

                    row[i] |= (1 << num);
                    col[j] |= (1 << num);
                    box[i/3*3+ j /3] |= (1<<num);

                    if(SolverRec(i, j + 1))
                    {
                        return true;
                    }

                    solvedSudoku[i * size + j] = '0';
                    row[i] &= ~(1 << num);
                    col[j] &= ~(1 << num);
                    box[i / 3 * 3 + j / 3] &= ~(1 << num);

                }
            }
            return false;
        }

        public void Solve()
        {
            SolverRec(0, 0);
            sudoku = new string(solvedSudoku);
        }

        public void printSudoku()
        {
            Console.WriteLine("-------------------------");
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (j % (int)Math.Sqrt(size) == 0)
                    {
                        Console.Write("| ");
                    }
                    Console.Write(sudoku[i*size+ j] == '0' ? " " : sudoku[i * size + j].ToString());
                    Console.Write(" ");
                }
                Console.WriteLine("|");

                if ((i + 1) % (int)Math.Sqrt(size) == 0)
                {
                    Console.WriteLine("-------------------------");
                }
            }
        }
    }
}
