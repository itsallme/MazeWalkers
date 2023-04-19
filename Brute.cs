using System;
using System.IO;
using System.Diagnostics;

namespace Algorithm_Analysis_Maze_Solver
{
    public class MazeSolver
    {
        private int[,] maze;
        private int width;
        private int height;

        public static float runtime;
        public static int squaresChecked;
        public static int pathLength;
        

        static void Main(string[] args)
        {
            string folderPath = "C:\\Users\\grady\\FileData";
            string fileName = "1000x1000 times 1000.csv";
            MazeSolveData data = new MazeSolveData();
            MazeSettings settings = new MazeSettings(
                1000,                 // width
                1000,                 // height
                10,                 // wallSize
                0,                  // removeWalls
                300,                // maxRemoveWalls
                0,                  // maxMaze
                0,                  // maxSolve
                "diagonal",         // entryType
                ""                  // bias
                );
            for(int i = 0; i<1000; i++)
            {
                Maze maze = new Maze(settings);

                maze.Generate();
                MazeSolver solver = new MazeSolver(maze.GetMatrixArray());

                EntryNodes enterExit = maze.GetEntryNodes(maze.entryNodes);
                int[] start = new int[] { enterExit.start.y, enterExit.start.x };
                int[] end = new int[] { enterExit.end.y, enterExit.end.x };

                DateTime startTime = DateTime.Now;
                bool solved = solver.Solve(start[1], start[0], end[1], end[0]);
                DateTime endTime = DateTime.Now;
                TimeSpan elapsedTime = endTime - startTime;
                runtime = (float)elapsedTime.TotalMilliseconds;
                data.AddData(runtime, squaresChecked, pathLength);
                squaresChecked = 0;
                pathLength = 0;
            }
            IOMachine IO = new IOMachine(folderPath, fileName);
            IO.WriteData(data.GetAllDataStrings().ToArray());

            /*Program.PrintMaze(maze.GetMatrixArray());

            if (solved)
            {
                Console.WriteLine("Maze solved!");
                Console.WriteLine("Runtime: " + runtime);
                Console.WriteLine("Squares checked: " + squaresChecked);
                Console.WriteLine("Path Length: " + pathLength);
            }
            else
            {
                Console.WriteLine("Maze unsolvable.");
            }*/
        }

        public MazeSolver(int[,] maze)
        {
            this.maze = maze;
            this.width = maze.GetLength(0);
            this.height = maze.GetLength(1);
        }

        public bool Solve(int startX, int startY, int endX, int endY)
        {
            squaresChecked++;
            // Check if the current position is the end position
            if (startX == endX && startY == endY)
            {
                return true;
            }

            // Check if the current position is out of bounds or a wall
            if (startX < 0 || startY < 0 || startX >= width || startY >= height || maze[startX, startY] == 1)
            {
                squaresChecked--;
                return false;
            }

            // Mark the current position as visited
            maze[startX, startY] = 1;

            // Recursively try all possible moves from the current position
            if (Solve(startX + 1, startY, endX, endY))
            {
                pathLength++;
                return true;
            }
            if (Solve(startX, startY + 1, endX, endY))
            {
                pathLength++;
                return true;
            }
            if (Solve(startX - 1, startY, endX, endY))
            {
                pathLength++;
                return true;
            }
            if (Solve(startX, startY - 1, endX, endY))
            {
                pathLength++;
                return true;
            }

            // Mark the current position as unvisited
            maze[startX, startY] = 0;

            return false;
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
