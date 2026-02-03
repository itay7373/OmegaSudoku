# Omega Sudoku Solver
Second Omega project - A super fast sudoku solver.

## Instructions
1. Download the project folder and open it in Visual Studio.
2. Ensure you have .Net10 installed on your computer.
3. Locate the `Program.cs` file and run the project.

**Usage:**
Enter a Sudoku board as a string with 81 characters where 0 represent an empty cell and press **Enter**.
The program will display the board and its solution, along with the total time taken to solve it.

Example for an input: "800000070006010053040600000000080400003000700020005038000000800004050061900002000"
<img width="660" height="516" alt="image" src="https://github.com/user-attachments/assets/eed4dbe5-f9b7-496c-834f-7702c21ad559" />
 

**Error Handling:**
If th solver can not solve your board for any reason, it will provide a detailed explanation.
> **Note:** For detailed explanations of the functions and logic, please refer to the documentation inside the code.

## Algorithm & Optimization
The solver utilizes a Backtracking algorithm, enhanced with several advanced heuristics and techniques to achieve high performance: 
 - **Bitmasking**: Implemented to represent available numbers in rows, columns, and boxes. This allows for extremely fast state checks and transitions using bitwise operations instead of iterating through arrays.
 - **Minimum Remaining Values (MRV)**: A powerful heuristic that directs the solver to the cell with the fewest possible legal values. If multiple cells have the same minimum count, 
 the algorithm prioritizes the cell with the highest degree (the one affecting the most empty neighbors) to prune the search tree earlier.
 - **Naked Single**: A human-centric technique where the algorithm automatically fills a cell if it has only one possible candidate remaining.
 - **Hidden Single**: The solver scans rows, columns, and 3x3 boxes for values that can only fit in a single cell. If such a value is found, it is placed immediately, significantly reducing the recursion depth.

For a deeper understanding of Sudoku solving techniques like Naked and Hidden Singles, check out this guide: https://www.sudokumood.com/learn/naked-single

## Built With
* C#
* .Net 10
* Visual Studio

## Author
Itay Efraim
