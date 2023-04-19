// Maze Generator
// Adapted to C# by Zane Baker
// Adapted from https://github.com/keesiemeijer/maze-generator
// I never want to convert from JavaScript again...

using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm_Analysis_Maze_Solver
{
	public class Maze
	{
		Random rand = new Random();

		public List<string> matrix;
		public int wallsRemoved, width, height, wallSize, removeWalls, maxMaze, maxCanvas, maxCanvasDimension, maxSolve, maxWallsRemove;
		public string entryNodes, bias, color, backgroundColor, solveColor;
		public EntryNodes finalEntryNodes;

		public Maze(MazeSettings settings)
		{
			MazeSettings defaults = new MazeSettings(20, 20, 10, 0, 300, 0, 0, "diagonal", "");

			if (settings == null)
			{
				this.matrix = new List<string>();
				this.wallsRemoved = 0;
				this.width = defaults.width;
				this.height = defaults.height;
				this.wallSize = defaults.wallSize;
				this.removeWalls = defaults.removeWalls;
				this.entryNodes = defaults.entryType;
				this.bias = defaults.bias;
				this.color = defaults.color;
				this.backgroundColor = defaults.backgroundColor;
				this.solveColor = defaults.solveColor;
				this.maxMaze = defaults.maxMaze;
				this.maxCanvas = defaults.maxCanvas;
				this.maxCanvasDimension = defaults.maxCanvasDimension;
				this.maxSolve = defaults.maxSolve;
				this.maxWallsRemove = defaults.maxWallsRemove;
			}
			else
			{
				this.matrix = new List<string>();
				this.wallsRemoved = 0;
				this.width = settings.width;
				this.height = settings.height;
				this.wallSize = settings.wallSize;
				this.removeWalls = settings.removeWalls;
				this.entryNodes = settings.entryType;
				this.bias = settings.bias;
				this.color = settings.color;
				this.backgroundColor = settings.backgroundColor;
				this.solveColor = settings.solveColor;
				this.maxMaze = settings.maxMaze;
				this.maxCanvas = settings.maxCanvas;
				this.maxCanvasDimension = settings.maxCanvasDimension;
				this.maxSolve = settings.maxSolve;
				this.maxWallsRemove = settings.maxWallsRemove;
			}
		}

		public void Generate()
		{
			if (!this.isValidSize())
			{
				this.matrix = null;
				Console.WriteLine("Please use smaller maze dimensions");
				return;
			}

			string[] nodes = GenerateNodes();
			nodes = ParseMaze(nodes);
			GetMatrix(nodes);
			RemoveMazeWalls();

			Console.WriteLine("Successfully Generated Maze");
		}

		public bool isValidSize()
		{
			int max = this.maxCanvasDimension;
			int canvas_width = ((this.width * 2) + 1) * this.wallSize;
			int canvas_height = ((this.height * 2) + 1) * this.wallSize;

			// Max dimension Firefox and Chrome
			if (max != 0 && ((max <= canvas_width) || (max <= canvas_height)))
			{
				return false;
			}

			// Max area (200 columns) * (200 rows) with wall size 10px
			if (this.maxCanvas != 0 && (this.maxCanvas <= (canvas_width * canvas_height)))
			{
				return false;
			}

			return true;
		}

		public string[] GenerateNodes()
		{
			int count = this.width * this.height;
			string[] nodes = new string[count];

			for (int i = 0; i < count; i++)
			{
				// visited, nswe
				nodes[i] = "01111";
			}

			return nodes;
		}

		public string[] ParseMaze(string[] nodes)
		{

			int mazeSize = nodes.Length;
			KeyedInt[] positionIndex = { new KeyedInt("n", 1), new KeyedInt("s", 2), new KeyedInt("w", 3), new KeyedInt("e", 4) };
			KeyedInt[] oppositeIndex = { new KeyedInt("n", 2), new KeyedInt("s", 1), new KeyedInt("w", 4), new KeyedInt("e", 3) };

			if (mazeSize == 0)
			{
				return null;
			}

			int max = 0;
			Stack<int> moveNodes = new Stack<int>();
			int visited = 0;
			int position = (int)(rand.NextDouble() * nodes.Length);
			Console.WriteLine($"Selected initial position: {position}");

			int biasCount = 0;
			int biasFactor = 3;
			if (this.bias != "")
			{
				if (("horizontal" == this.bias))
				{
					biasFactor = (1 <= (this.width / 100)) ? (this.width / 100) + 2 : 3;
				}
				else if ("vertical" == this.bias)
				{
					biasFactor = (1 <= (this.height / 100)) ? (this.height / 100) + 2 : 3;
				}
			}

			// Set start node visited.
			nodes[position] = ReplaceAt(nodes[position], 0, '1');

			while (visited < (mazeSize - 1))
			{
				biasCount++;

				max++;
				if (this.maxMaze != 0 && (this.maxMaze < max))
				{
					Console.WriteLine("Please use smaller maze dimensions");
					moveNodes = null;
					this.matrix = null;
					return null;
				}

				KeyedInt[] next = GetNeighbours(position);
				List<KeyedInt> directions = new List<KeyedInt>();

				foreach (KeyedInt item in next)
				{
                    if (item.value != -1)
                    {
						if (nodes[item.value][0].Equals('0'))
						{
							//Console.WriteLine("Adding direction");
							directions.Add(item);
						}
					}
				}

				/*
				Console.Write($"Valid directions at position {position}: ");
				foreach (KeyedInt item in directions)
                {
					Console.Write($"{item.key}, ");
                }
				Console.WriteLine();
				*/

				if (this.bias != "" && (biasCount != biasFactor))
				{
					directions = BiasDirections(directions);
				}
				else
				{
					biasCount = 0;
				}

				if (directions.Count > 0)
				{
					++visited;

					if (1 < directions.Count)
					{
						moveNodes.Push(position);
					}

					KeyedInt direction = directions[(int)(rand.NextDouble() * directions.Count)];

					int directionIndex = 0;

					switch (direction.key)
					{
						case "n":
							directionIndex = 0;
							break;
						case "s":
							directionIndex = 1;
							break;
						case "w":
							directionIndex = 2;
							break;
						case "e":
							directionIndex = 3;
							break;
					}

					//Console.WriteLine($"directionIndex = {directionIndex}");

					// Update current position
					nodes[position] = ReplaceAt(nodes[position], positionIndex[directionIndex].value, '0');

					// Set new position
					position = next[directionIndex].value;

					// Update next position
					nodes[position] = ReplaceAt(nodes[position], oppositeIndex[directionIndex].value, '0');
					nodes[position] = ReplaceAt(nodes[position], 0, '1');
				}
				else
				{
					if (moveNodes.Count == 0)
					{
						Console.WriteLine("No directions given and movenodes == 0");
						break;
					}

					position = moveNodes.Pop();
				}
			}

			return nodes;
		}

		public void GetMatrix(string[] nodes)
		{
			int mazeSize = this.width * this.height;

			// Add the complete maze in a matrix
			// where 1 is a wall and 0 is a corridor.

			string row1 = "";
			string row2 = "";

			if (nodes.Length != mazeSize)
			{
				return;
			}

			for (int i = 0; i < mazeSize; i++)
			{
				row1 += row1.Length == 0 ? "1" : "";
				row2 += row2.Length == 0 ? "1" : "";

				Console.WriteLine($"Current node in position {i}: {nodes[i]}");

				if ((nodes[i][1]).Equals('1'))
				{
					Console.WriteLine("At top of maze, adding walls");
					row1 += "11";
					if ((nodes[i][4]).Equals('1'))
					{
						Console.WriteLine("Top right of maze");
						row2 += "01";
					}
					else
					{
						row2 += "00";
					}
				}
				else
				{
					bool hasAbove = (i - this.width >= 0);														//nodes.hasOwnProperty(i - this.width);
					bool above = hasAbove && (nodes[i - this.width][4]).Equals('1');
					bool hasNext = (i + 1 < nodes.Length);														//nodes.hasOwnProperty(i + 1);
					bool next = hasNext && (nodes[i + 1][1]).Equals('1');
					Console.WriteLine($"i = {i}, hasAbove = {hasAbove}, above = {above}, hasNext = {hasNext}, next = {next}");

					if ((nodes[i][4]).Equals('1'))
					{
						row1 += "01";
						row2 += "01";
					}
					else if (next || above)
					{
						row1 += "01";
						row2 += "00";
					}
					else
					{
						row1 += "00";
						row2 += "00";
					}
				}

				if (0 == ((i + 1) % this.width))
				{
					this.matrix.Add(row1);
					this.matrix.Add(row2);
					row1 = "";
					row2 = "";
				}
			}

			// Add closing row
			string finalRowTemp = "";
			for (int i = 0; i < (this.width * 2) + 1; i++)
			{
				finalRowTemp += "1";
			}
			this.matrix.Add(finalRowTemp);
		}

		public EntryNodes GetEntryNodes(string access)
		{
			int y = ((this.height * 2) + 1) - 2;
			int x = ((this.width * 2) + 1) - 2;

			EntryNodes entryNodes = null;

			if ("diagonal" == access)
			{
				entryNodes = new EntryNodes(
					new EntryNodes.NodeData(1, 1, new int[] { 0, 1 }),
					new EntryNodes.NodeData(x, y, new int[] { x + 1, y })
					);
			}

			if ("horizontal" == access || "vertical" == access)
			{
				int xy = ("horizontal" == access) ? y : x;
				xy = ((xy - 1) / 2);
				bool even = (xy % 2 == 0);
				xy = even ? xy + 1 : xy;

				int start_x = ("horizontal" == access) ? 1 : xy;
				int start_y = ("horizontal" == access) ? xy : 1;
				int end_x = ("horizontal" == access) ? x : (even ? start_x : start_x + 2);
				int end_y = ("horizontal" == access) ? (even ? start_y : start_y + 2) : y;
				int[] startgate = ("horizontal" == access) ? new int[] { 0, start_y } : new int[] { start_x, 0 };
				int[] endgate = ("horizontal" == access) ? new int[] { x + 1, end_y } : new int[] { end_x, y + 1 };

				entryNodes = new EntryNodes(
					new EntryNodes.NodeData(start_x, start_y, startgate),
					new EntryNodes.NodeData(end_x, end_y, endgate)
					);
			}

			return entryNodes;
		}

		public List<KeyedInt> BiasDirections(List<KeyedInt> directions)
		{
			bool horizontal = false;
			bool vertical = false;

			foreach (KeyedInt item in directions)
			{
				if ((item.key == "w" || item.key == "e"))
				{
					horizontal = true;
				}
				if (item.key == "n" || item.key == "s")
				{
					vertical = true;
				}
			}

			if (("horizontal" == this.bias) && horizontal)

			{
				for (int i = 0; i < directions.Count; i++)
				{
					if (directions[i].key == "n" || directions[i].key == "s")
					{
						directions.RemoveAt(i);
						i--;
					}
				}
			}
			else if (("vertical" == this.bias) && vertical)
			{
				for (int i = 0; i < directions.Count; i++)
				{
					if (directions[i].key == "w" || directions[i].key == "e")
					{
						directions.RemoveAt(i);
						i--;
					}
				}
			}

			return directions;
		}

		public KeyedInt[] GetNeighbours(int pos)
		{
			KeyedInt[] temp = new KeyedInt[4];

			temp[0] = new KeyedInt("n", (0 <= (pos - this.width)) ? pos - this.width : -1);
			temp[1] = new KeyedInt("s", ((this.width * this.height) > (pos + this.width)) ? pos + this.width : -1);
			temp[2] = new KeyedInt("w", ((0 < pos) && (0 != (pos % this.width))) ? pos - 1 : -1);
			temp[3] = new KeyedInt("e", (0 != ((pos + 1) % this.width)) ? pos + 1 : -1);

			return temp;
		}

		public bool RemoveWall(int row, int index)
		{
			// Remove wall if possible.
			bool evenRow = (row % 2 == 0);
			bool evenIndex = (index % 2 == 0);
			int wall = (int)(this.matrix[row][index]);

			if (wall == 0)
			{
				return false;
			}

			if (!evenRow && evenIndex)
			{
				// Uneven row and even column
				bool hasTop = (row - 2 > 0) && (this.matrix[row - 2][index]).Equals('1');
				bool hasBottom = (row + 2 < this.matrix.Count) && (this.matrix[row + 2][index]).Equals('1');

				if (hasTop && hasBottom)
				{
					this.matrix[row] = ReplaceAt(this.matrix[row], index, '0');
					return true;
				}
				else if (!hasTop && hasBottom)
				{
					bool left = (this.matrix[row - 1][index - 1]).Equals('1');
					bool right = (this.matrix[row - 1][index + 1]).Equals('1');
					if (left || right)
					{
						this.matrix[row] = ReplaceAt(this.matrix[row], index, '0');
						return true;
					}
				}
				else if (!hasBottom && hasTop)
				{
					bool left = (this.matrix[row + 1][index - 1]).Equals('1');
					bool right = (this.matrix[row + 1][index + 1]).Equals('1');
					if (left || right)
					{
						this.matrix[row] = ReplaceAt(this.matrix[row], index, '0');
						return true;
					}
				}

			}
			else if (evenRow && !evenIndex)
			{
				// Even row and uneven column
				bool hasLeft = (this.matrix[row][index - 2]).Equals('1');
				bool hasRight = (this.matrix[row][index + 2]).Equals('1');

				if (hasLeft && hasRight)
				{
					this.matrix[row] = ReplaceAt(this.matrix[row], index, '0');
					return true;
				}
				else if (!hasLeft && hasRight)
				{
					bool top = (this.matrix[row - 1][index - 1]).Equals('1');
					bool bottom = (this.matrix[row + 1][index - 1]).Equals('1');
					if (top || bottom)
					{
						this.matrix[row] = ReplaceAt(this.matrix[row], index, '0');
						return true;
					}
				}
				else if (!hasRight && hasLeft)
				{
					bool top = (this.matrix[row - 1][index + 1]).Equals('1');
					bool bottom = (this.matrix[row + 1][index + 1]).Equals('1');
					if (top || bottom)
					{
						this.matrix[row] = ReplaceAt(this.matrix[row], index, '0');
						return true;
					}
				}
			}

			return false;
		}

		public void RemoveMazeWalls()
		{
			if (this.removeWalls == 0 || this.matrix.Count == 0)
			{
				return;
			}

			int min = 1;
			int max = this.matrix.Count - 1;
			int maxTries = this.maxWallsRemove;
			int tries = 0;

			while (tries < maxTries)
			{
				tries++;

				// Did we reached the goal
				if (this.wallsRemoved >= this.removeWalls)
				{
					break;
				}

				// Get random row from matrix
				int y = (int)(rand.NextDouble() * (max - min + 1)) + min;
				y = (y == max) ? y - 1 : y;

				List<int> walls = new List<int>();
				string row = this.matrix[y];

				// Get walls from random row
				for (int i = 0; i < row.Length; i++)
				{
					if (i == 0 || i == row.Length - 1)
					{
						continue;
					}

					int wall = (int)(row[i]);
					if (wall != 0)
					{
						walls.Add(i);
					}
				}

				// Shuffle walls randomly
				ShuffleArray(walls);

				// Try breaking a wall for this row.
				for (int i = 0; i < walls.Count; i++)
				{
					if (this.RemoveWall(y, walls[i]))
					{

						// Wall can be broken
						this.wallsRemoved++;
						break;
					}
				}
			}
		}

		public string ReplaceAt(string stringReplace, int index, char replace)
		{
			if (index >= stringReplace.Length)
			{
				return stringReplace;
			}

			return stringReplace.Substring(0, index) + replace + stringReplace.Substring(index + 1);
		}

		public void ShuffleArray(List<int> array)
		{
			for (int i = array.Count - 1; i > 0; i--)
			{
				int j = (int)(rand.NextDouble() * (i + 1));
				int temp = array[i];
				array[i] = array[j];
				array[j] = temp;
			}
		}

		public int[,] GetMatrixArray()
        {
			int[,] matrixArray = new int[matrix.Count,matrix[0].Length];
            for (int row = 0; row < matrix.Count; row++)
            {
                for (int column = 0; column < (matrix[row]).Length; column++)
                {
                    switch ((matrix[row])[column])
                    {
						case '1':
							matrixArray[row, column] = 1;
							break;
						case '0':
							matrixArray[row, column] = 0;
							break;
                    }
                }
            }

			return matrixArray;
        }
	}

    public class MazeSettings
    {
        public int width, height, wallSize, removeWalls, maxWallsRemove, maxMaze, maxSolve;
		public string entryType, bias;

		public int maxCanvas = 0;
		public int maxCanvasDimension = 0;
		public string color = "#000000";
		public string backgroundColor = "#FFFFFF";
		public string solveColor = "#cc3737";


        public MazeSettings(int width, int height, int wallSize, int removeWalls, int maxWallsRemove, int maxMaze, int maxSolve, string entryType, string bias)
        {
            this.width = width;
            this.height = height;
            this.wallSize = wallSize;
            this.removeWalls = removeWalls;
            this.maxWallsRemove = maxWallsRemove;
            this.maxMaze = maxMaze;
            this.maxSolve = maxSolve;
            this.entryType = entryType;
            this.bias = bias;
        }
    }

	public class EntryNodes
    {
		public NodeData start;
		public NodeData end;

		public EntryNodes(NodeData start, NodeData end)
        {
			this.start = start;
			this.end = end;
        }

		public class NodeData
        {
			public int x;
			public int y;
			public int[] gate;

			public NodeData(int x, int y, int[] gate)
            {
				this.x = x;
				this.y = y;
				this.gate = gate;
            }
        }
    }

	public class KeyedInt
    {
		public int value;
		public string key;

		public KeyedInt(string key, int value)
        {
			this.key = key;
			this.value = value;
        }
    }
}
