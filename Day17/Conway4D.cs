using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Day17
{
    class Conway4D
    {
        public Dictionary<(int x, int y, int z, int w), int> Neighbors;
        public HashSet<(int x, int y, int z, int w)> ActiveCubes;
        public HashSet<(int x, int y, int z, int w)> TempActiveCubes;
        public static readonly List<(int x, int y, int z, int w)> Offsets = new List<(int x, int y, int z, int w)>()
        {
            (1, 1, 0, 0), (1, 0, 0, 0), (1, -1, 0, 0), (0, -1, 0, 0), (-1, -1, 0, 0), (-1, 0, 0, 0), (-1, 1, 0, 0), (0, 1, 0, 0), (0, 0, 1, 0), (1, 1, 1, 0), (1, 0, 1, 0), (1, -1, 1, 0), (0, -1, 1, 0), (-1, -1, 1, 0), (-1, 0, 1, 0), (-1, 1, 1, 0), (0, 1, 1, 0), (0, 0, -1, 0), (1, 1, -1, 0), (1, 0, -1, 0), (1, -1, -1, 0), (0, -1, -1, 0), (-1, -1, -1, 0), (-1, 0, -1, 0), (-1, 1, -1, 0), (0, 1, -1, 0),
            (0, 0, 0, 1), (1, 1, 0, 1), (1, 0, 0, 1), (1, -1, 0, 1), (0, -1, 0, 1), (-1, -1, 0, 1), (-1, 0, 0, 1), (-1, 1, 0, 1), (0, 1, 0, 1), (0, 0, 1, 1), (1, 1, 1, 1), (1, 0, 1, 1), (1, -1, 1, 1), (0, -1, 1, 1), (-1, -1, 1, 1), (-1, 0, 1, 1), (-1, 1, 1, 1), (0, 1, 1, 1), (0, 0, -1, 1), (1, 1, -1, 1), (1, 0, -1, 1), (1, -1, -1, 1), (0, -1, -1, 1), (-1, -1, -1, 1), (-1, 0, -1, 1), (-1, 1, -1, 1),
            (0, 1, -1, 1), (0, 0, 0, -1), (1, 1, 0, -1), (1, 0, 0, -1), (1, -1, 0, -1), (0, -1, 0, -1), (-1, -1, 0, -1), (-1, 0, 0, -1), (-1, 1, 0, -1), (0, 1, 0, -1), (0, 0, 1, -1), (1, 1, 1, -1), (1, 0, 1, -1), (1, -1, 1, -1), (0, -1, 1, -1), (-1, -1, 1, -1), (-1, 0, 1, -1), (-1, 1, 1, -1), (0, 1, 1, -1), (0, 0, -1, -1), (1, 1, -1, -1), (1, 0, -1, -1), (1, -1, -1, -1), (0, -1, -1, -1), (-1, -1, -1, -1), (-1, 0, -1, -1), (-1, 1, -1, -1), (0, 1, -1, -1)
        };
        
        public Conway4D(string input)
        {
            Neighbors = new Dictionary<(int x, int y, int z, int w), int>();
            ActiveCubes = new HashSet<(int x, int y, int z, int w)>();
            TempActiveCubes = new HashSet<(int x, int y, int z, int w)>();

            string[] rows = input.Split('\n');
            for (int row = 0; row < rows.Length; row++)
            {
                for (int col = 0; col < rows[row].Length; col++)
                {
                    if (rows[row][col] == '#')
                    {
                        ActiveCubes.Add((row, col, 0, 0));
                    }
                }
            }           
        }

        public int GetActiveCubesAfterCycles4D(int totalCycles)
        {
            for (int i = 0; i < totalCycles; i++)
            {
                RunConway4D();
            }

            return ActiveCubes.Count;
        }

        public void RunConway4D()
        {
            Neighbors.Clear();
            foreach ((int x, int y, int z, int w) activeCube in ActiveCubes)
            {
                GetNeighbors(activeCube);
            }

            TempActiveCubes.Clear();
            foreach (((int x, int y, int z, int w) cube, int activeCount) in Neighbors)
            {
                if (ActiveCubes.Contains(cube) && (activeCount == 2 || activeCount == 3))
                {
                    TempActiveCubes.Add(cube);
                }
                else if (activeCount == 3)
                {
                    TempActiveCubes.Add(cube);
                }
            }

            HashSet<(int x, int y, int z, int w)> temp = ActiveCubes;
            ActiveCubes = TempActiveCubes;
            TempActiveCubes = temp;
        }

        public void GetNeighbors((int x, int y, int z, int w) activeCube)
        {
            foreach ((int x, int y, int z, int w) offset in Offsets)
            {
                (int x, int y, int z, int w) position = (activeCube.x + offset.x, activeCube.y + offset.y, activeCube.z + offset.z, activeCube.w + offset.w);
                if (!Neighbors.ContainsKey(position))
                {
                    Neighbors[position] = 1;
                }
                else Neighbors[position]++;
            }
        }
    }
}
