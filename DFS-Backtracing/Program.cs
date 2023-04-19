using System;
using System.Collections.Generic;
using System.IO;

namespace Algorithm_Analysis_Maze_Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            string filepath = "C:\\Users\\zaneb\\source\\repos\\Algorithm Analysis Maze Solver\\Algorithm Analysis Maze Solver\\DataOutputs\\";
            string filename = "10x10 times 1000.csv";

            int numOfTrials = 1000;

            MazeSettings settings = new MazeSettings(
                10,               // width
                10,               // height
                10,                 // wallSize
                0,                  // removeWalls
                300,                // maxRemoveWalls
                0,                  // maxMaze
                0,                  // maxSolve
                "diagonal",         // entryType
                ""                  // bias
                );
            Maze maze = new Maze(settings);

            MazeSolverAgent solver;

            MazeSolveData data = new MazeSolveData();


            ////////////////////////////////////////////////
            //                10x10 * 1000                //
            ////////////////////////////////////////////////

            for (int i = 0; i < numOfTrials; i++)
            {
                maze = new Maze(settings);
                maze.Generate();
                EntryNodes enterExit = maze.GetEntryNodes(maze.entryNodes);
                int[] start = new int[] { enterExit.start.y, enterExit.start.x };
                int[] end = new int[] { enterExit.end.y, enterExit.end.x };
                
                solver = new MazeSolverAgent(maze.GetMatrixArray(), start, end);
                
                solver.SolveMaze(false);

                data.AddData(solver.lastRuntime, solver.lastSquaresChecked, solver.lastPathLength);

                //PrintMaze(maze.GetMatrixArray());
                //Console.WriteLine($"Data Collected: {data.GetDataString(data.GetDataCount() - 1)}");
                Console.WriteLine(i);
            }

            IOMachine outputMachine = new IOMachine(filepath, filename);
            outputMachine.WriteData(data.GetAllDataStrings().ToArray());

            
            ////////////////////////////////////////////////
            //                100x100 * 1000              //
            ////////////////////////////////////////////////

            settings.width = 100;
            settings.height = 100;
            filename = "100x100 times 1000.csv";

            for (int i = 0; i < numOfTrials; i++)
            {
                maze = new Maze(settings);
                maze.Generate();
                EntryNodes enterExit = maze.GetEntryNodes(maze.entryNodes);
                int[] start = new int[] { enterExit.start.y, enterExit.start.x };
                int[] end = new int[] { enterExit.end.y, enterExit.end.x };

                solver = new MazeSolverAgent(maze.GetMatrixArray(), start, end);

                solver.SolveMaze(false);

                data.AddData(solver.lastRuntime, solver.lastSquaresChecked, solver.lastPathLength);

                PrintMaze(maze.GetMatrixArray());
                Console.WriteLine($"Data Collected: {data.GetDataString(data.GetDataCount() - 1)}");
                Console.WriteLine();
            }

            outputMachine.fileName = filename;
            outputMachine.WriteData(data.GetAllDataStrings().ToArray());
            
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

    public class IOMachine
    {
        public string folderPath;
        public string fileName;

        public IOMachine(string folderPath, string fileName)
        {
            if (!folderPath[folderPath.Length - 1].Equals('\\'))
            {
                folderPath += '\\';
            }

            this.folderPath = folderPath;
            this.fileName = fileName;
        }

        public void WriteData(string[] data)
        {
            string fullPath = folderPath + fileName;

            if (!File.Exists(fullPath))
            {
                File.Create(fullPath).Close();
            }

            File.WriteAllLines(fullPath, data);
        }
    }
}
