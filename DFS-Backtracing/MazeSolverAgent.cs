using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm_Analysis_Maze_Solver
{
    class MazeSolverAgent
    {
        private int[] position;     // Position in (row, column)
        private int[] start;        // Start in (row, column)
        private int[] end;          // End in (row, column)
        private int[,] mazeCopy;    // Maze as a nxm array

        private bool reachedEnd;

        public float lastRuntime;
        public int lastPathLength;
        public int lastSquaresChecked;

        /// <summary>
        /// Creates new instance of a maze solver agent.
        /// </summary>
        /// <param name="maze">An nxm array representing the maze to be solved in (row, column) form.</param>
        /// <param name="start">The start point in (row, column) form.</param>
        /// <param name="end">The end point in (row, column) form.</param>
        public MazeSolverAgent(int[,] maze, int[] start, int[] end)
        {
            this.mazeCopy = CopyMaze(maze);
            ConvertMaze();
            this.start = start;
            this.position = start;
            this.end = end;

            lastRuntime = 0;
            lastPathLength = 0;
            lastSquaresChecked = 0;

            reachedEnd = false;
        }

        public void NewMaze(int[,] newMaze, int[] newStart, int[] newEnd)
        {
            this.mazeCopy = CopyMaze(newMaze);
            this.start = newStart;
            this.position = newStart;
            this.end = newEnd;

            reachedEnd = false;
        }

        /// <summary>
        /// Completes the next step of the agent's maze traversal.
        /// </summary>
        /// <returns>True if next step was successful, False if no valid position was found.</returns>
        public bool NextStep()
        {
            reachedEnd = CheckEnd();
            if (reachedEnd)
            {
                return true;
            }

            // See if the forward path is possible
            bool success = Forward();

            lastPathLength++;
            lastSquaresChecked++;

            // If the forward path is not possible, try backtracking
            if (!success)
            {
                success = Backward();
                lastPathLength--;
            }

            // Set current position to 4 to mark location.
            mazeCopy[position[0], position[1]] = 4;

            // Return the result of the step
            return success;

        }

        /// <summary>
        /// Attempts to make forward progress in the maze.
        /// </summary>
        /// <returns>True if the agent could make forward progress, False if not.</returns>
        private bool Forward()
        {
            int row, column;
            bool zeroFound = false;


            // Check left
            row = position[0];
            column = position[1] - 1;

            // Ensure the position is a valid position
            if (column >= 0)
            {
                // Check for empty tiles left of the current position
                zeroFound = TryAdvance(row, column);

                // If successful, return true
                if (zeroFound)
                {
                    return true;
                }
            }


            // Check up
            row = position[0] - 1;
            column = position[1];

            // Ensure the position is a valid position
            if (row >= 0)
            {
                // Check for empty tiles above the current position
                zeroFound = TryAdvance(row, column);

                // If successful, return true
                if (zeroFound)
                {
                    return true;
                }
            }


            // Check right
            row = position[0];
            column = position[1] + 1;

            // Ensure the position is a valid position
            if (column < mazeCopy.GetLength(1))
            {
                // Check for empty tiles right of the current position
                zeroFound = TryAdvance(row, column);

                // If successful, return true
                if (zeroFound)
                {
                    return true;
                }
            }


            // Check down
            row = position[0] + 1;
            column = position[1];

            // Ensure the position is a valid position
            if (row < mazeCopy.GetLength(0))
            {
                // Check for empty tiles below the current position
                zeroFound = TryAdvance(row, column);

                // If successful, return true
                if (zeroFound)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to advance forward to the given location at maze[row, column].
        /// </summary>
        /// <param name="row">The row to advance to</param>
        /// <param name="column">The column to advance to</param>
        /// <returns>True if agent successfully advanced to an unexplored tile, False if not.</returns>
        private bool TryAdvance(int row, int column)
        {
            // If the tile is unexplored, set current tile to 1 and advance to the next tile
            if (mazeCopy[row, column] == 0)
            {
                mazeCopy[position[0], position[1]] = 1;
                position[0] = row;
                position[1] = column;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to backtrack along previously traveled tiles.
        /// </summary>
        /// <returns>True if backtrack was successful, False if not.</returns>
        private bool Backward()
        {
            int row, column;
            bool tileFound = false;


            // Check down
            row = position[0] + 1;
            column = position[1];

            // Ensure the position is a valid position
            if (row < mazeCopy.GetLength(0))
            {
                // Check for previous tiles below the current position
                tileFound = TryBacktrack(row, column);

                // If successful, return true
                if (tileFound)
                {
                    return true;
                }
            }


            // Check right
            row = position[0];
            column = position[1] + 1;

            // Ensure the position is a valid position
            if (column < mazeCopy.GetLength(1))
            {
                // Check for previous tiles right of the current position
                tileFound = TryBacktrack(row, column);

                // If successful, return true
                if (tileFound)
                {
                    return true;
                }
            }


            // Check up
            row = position[0] - 1;
            column = position[1];

            // Ensure the position is a valid position
            if (row >= 0)
            {
                // Check for previous tiles above the current position
                tileFound = TryBacktrack(row, column);

                // If successful, return true
                if (tileFound)
                {
                    return true;
                }
            }


            // Check left
            row = position[0];
            column = position[1] - 1;

            // Ensure the position is a valid position
            if (column >= 0)
            {
                // Check for previous tiles left of the current position
                tileFound = TryBacktrack(row, column);

                // If successful, return true
                if (tileFound)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tries to backtrack to the given location at maze[row, column].
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns>True if agent successfully backtracked to an previous tile, False if not.</returns>
        private bool TryBacktrack(int row, int column)
        {
            // If the tile is a previous path, set current tile to 2 and backtrack to the previous tile
            if (mazeCopy[row, column] == 1)
            {
                mazeCopy[position[0], position[1]] = 2;
                position[0] = row;
                position[1] = column;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the agent has reached the endpoint of the maze.
        /// </summary>
        /// <returns>True if end has been reached, False if not.</returns>
        private bool CheckEnd()
        {
            if (position[0] == end[0] && position[1] == end[1])
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Detects whether or not the agent has reached the endpoint of the maze.
        /// </summary>
        /// <returns>True if agent has reached the end, False if not.</returns>
        public bool ReachedEnd()
        {
            return reachedEnd;
        }

        /// <summary>
        /// Gets the working copy of the maze from the agent.
        /// </summary>
        /// <returns>The working copy of the maze.</returns>
        public int[,] GetMaze()
        {
            return mazeCopy;
        }

        /// <summary>
        /// Copies an nxm array and returns the copy
        /// </summary>
        /// <param name="maze">The nxm array to copy</param>
        /// <returns></returns>
        private int[,] CopyMaze(int[,] maze)
        {
            int[,] copy = new int[maze.GetLength(0), maze.GetLength(1)];

            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int column = 0; column < maze.GetLength(1); column++)
                {
                    copy[row, column] = maze[row, column];
                }
            }

            return copy;
        }

        /// <summary>
        /// Solves the mase with steps optionally separated by enter presses.
        /// </summary>
        /// <param name="stepWise">Whether to do the process step by step.</param>
        public void SolveMaze(bool stepWise)
        {
            bool stepSuccess = true;

            lastRuntime = 0;
            DateTime startTime = DateTime.Now;
            DateTime endTime;

            while (!reachedEnd && stepSuccess)
            {
                if (stepWise)
                {
                    PrintMaze(this.mazeCopy);
                    Console.WriteLine("Press enter to advance to next step...");
                    Console.ReadLine();
                }

                stepSuccess = NextStep();
            }

            // Save runtime before debug output
            endTime = DateTime.Now;

            lastRuntime = endTime.Millisecond - startTime.Millisecond;


            if (!stepSuccess)
            {
                Console.WriteLine("Agent could not find end of maze. Solution may not exist.");
            }
            else if (reachedEnd)
            {
                Console.WriteLine("End of maze found!");
            }
        }

        /// <summary>
        /// Takes a nxm array of integers and prints the values in a grid. If the integer is 4, then print a * instead. If the integer is 5, then print a # instead
        /// </summary>
        /// <param name="maze">The nxm array to be printed</param>
        public static void PrintMaze(int[,] maze)
        {
            for (int row = 0; row < maze.GetLength(0); row++)
            {
                for (int column = 0; column < maze.GetLength(1); column++)
                {
                    if (maze[row, column] == 4)
                    {
                        Console.Write($"| * ");
                    }
                    else if (maze[row, column] == 5)
                    {
                        Console.Write($"| # ");
                    }
                    else
                    {
                        Console.Write($"| {maze[row, column]} ");
                    }
                }
                Console.WriteLine("|");
            }
        }

        /// <summary>
        /// Converts maze from 0 = empty and 1 = wall to 0 = empty and 5 = wall
        /// </summary>
        public void ConvertMaze()
        {
            for (int row = 0; row < mazeCopy.GetLength(0); row++)
            {
                for (int column = 0; column < mazeCopy.GetLength(1); column++)
                {
                    if (mazeCopy[row, column] == 1)
                    {
                        mazeCopy[row, column] = 5;
                    }
                }
            }
        }
    }
}
