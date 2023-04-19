using System;
using System.Collections.Generic;

namespace Algorithm_Analysis_Maze_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            MazeSettings settings = new MazeSettings(
                10,                 // width
                10,                 // height
                10,                 // wallSize
                0,                  // removeWalls
                300,                // maxRemoveWalls
                0,                  // maxMaze
                0,                  // maxSolve
                "diagonal",         // entryType
                ""                  // bias
                );
            Maze maze = new Maze(settings);

            maze.Generate();

            PrintMatrix(maze.matrix);
            PrintMaze(maze.GetMatrixArray());
        }

        public static void PrintMatrix(List<string> matrix)
        {
            foreach (string item in matrix)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i].Equals('1'))
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
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
                    if (maze[row, column] == 0)
                    {
                        Console.Write($".");
                    }
                    else if (maze[row, column] == 1)
                    {
                        Console.Write($"#");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
