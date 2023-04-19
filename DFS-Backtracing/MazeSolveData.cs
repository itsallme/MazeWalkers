using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm_Analysis_Maze_Solver
{
    class MazeSolveData
    {
        private List<DataNode> data;

        public MazeSolveData()
        {
            data = new List<DataNode>();
        }

        public void AddData(float runtime, int squaresChecked, int pathLength)
        {
            data.Add(new DataNode(runtime, squaresChecked, pathLength));
        }

        public void RemoveData(int index)
        {
            if (index >= data.Count || index < 0)
            {
                return;
            }

            data.RemoveAt(index);
        }

        public int GetDataCount()
        {
            return data.Count;
        }

        public string GetDataString(int index)
        {
            if (index >= data.Count || index < 0)
            {
                return null;
            }

            string temp = $"{data[index].runtime},{data[index].squaresChecked},{data[index].pathLength}";

            return temp;
        }

        public List<string> GetAllDataStrings()
        {
            if (data.Count == 0)
            {
                return null;
            }

            List<string> returnList = new List<string>();
            string temp = "";

            for (int i = 0; i < data.Count; i++)
            {
                temp = GetDataString(i);
                returnList.Add(temp);
            }

            return returnList;
        }

        private class DataNode
        {
            public float runtime;
            public int squaresChecked;
            public int pathLength;

            public DataNode()
            {
                runtime = 0;
                squaresChecked = 0;
                pathLength = 0;
            }

            public DataNode(float runtime, int squaresChecked, int pathLength)
            {
                this.runtime = runtime;
                this.squaresChecked = squaresChecked;
                this.pathLength = pathLength;
            }
        }
    }
}
