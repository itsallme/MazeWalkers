readme

To run the brute force search, 'Brute', you fill out the maze settings in the main method and run the program.
You also need to have the Maze.cs to be able to create a maze at all. 
Make sure to alter the file path for each time you run it so that it doesn't just add on to the existing file.

To run the DFS Backtracker, "MazeSolverAgent.cs", you must first feed it a maze using the NewMaze() method.
This method takes in an integer array of size NxM where 1s are walls and 0s are valid paths.
Once the maze solver has a maze, you can run Solve(), and the boolean entered determines whether the maze runs stepwise in the console.
After running Solve(), you can access all of the last solved maze data through lastRuntime, lastPathLength, and lastSquaresChecked.

Maze.cs is a maze generator with various settings that you can feed it, reference one of our example programs to get an idea of how the maze gen settings work.

MazeSolveData.cs is a simple data structure that can be used to store large amounts of maze solve data, allowing for multiple trial runs to be stored.
