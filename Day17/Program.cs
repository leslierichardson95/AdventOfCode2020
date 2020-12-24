using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.CompilerServices;

namespace Day17
{
    class Program
    {
        static Dictionary<(int x, int y, int z), int> Neighbors;
        static HashSet<(int x, int y, int z)> ActiveCubes;
        static HashSet<(int x, int y, int z)> TempActiveCubes;
        static List<(int x, int y, int z)> Offsets = new List<(int x, int y, int z)>()
        {
            (0,0,1), (0,1,0), (0,1,1), (1,0,0), (1,0,1), (1,1,0), (1,1,1),
            (0,0,-1), (0,-1,0), (0,-1,-1), (-1,0,0), (-1,0,-1), (-1,-1,0), (-1,-1,-1),
            (0,-1,1), (0,1,-1), (-1,0,1), (1,0,-1), (-1,1,0), (1,-1,0), 
            (-1,1,1), (1,-1,1), (1,1,-1), (-1,-1,1), (-1,1,-1), (1,-1,-1)
        };

        static void Main(string[] args)
        {
            string text = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day17/input.txt");
            InitializeConway(text);
            Console.WriteLine($"Remaining Active Cubes: {GetActiveCubesAfterCycles(6)}");

            Conway4D conway4D = new Conway4D(text);
            Console.WriteLine($"Remaining Active 4D Cubes: {conway4D.GetActiveCubesAfterCycles4D(6)}");
        }

        static void InitializeConway(string text)
        {
            ActiveCubes = new HashSet<(int x, int y, int z)>();
            TempActiveCubes = new HashSet<(int x, int y, int z)>();
            Neighbors = new Dictionary<(int x, int y, int z), int>();

            string[] rows = text.Split('\n');
            for (int row = 0; row < rows.Length; row++)
            {
                for (int col = 0; col < rows[row].Length; col++)
                {
                    if (rows[row][col] == '#')
                    {
                        ActiveCubes.Add((row, col, 0));
                    }
                }
            }
        }

        static int GetActiveCubesAfterCycles(int totalCycles)
        {
            for (int i = 0; i < totalCycles; i ++)
            {
                RunConway();
            }
            return ActiveCubes.Count;
        }

        static void RunConway()
        {
            Neighbors.Clear();
            foreach ((int x, int y, int z) activeCube in ActiveCubes)
            {
                GetNeighbors(activeCube);
            }

            TempActiveCubes.Clear();
            foreach (((int x, int y, int z) cube, int activeCount) in Neighbors)
            {
                if (ActiveCubes.Contains(cube) && (activeCount == 2 || activeCount == 3))
                {
                    TempActiveCubes.Add(cube);
                }
                else if (activeCount == 3) TempActiveCubes.Add(cube);
            }

            HashSet<(int x, int y, int z)> temp = ActiveCubes;
            ActiveCubes = TempActiveCubes;
            TempActiveCubes = temp;
        }

        static void GetNeighbors((int x, int y, int z) cube)
        {
            foreach ((int x, int y, int z) offset in Offsets)
            {
                (int x, int y, int z) position = (cube.x + offset.x, cube.y + offset.y, cube.z + offset.z);
                if (!Neighbors.ContainsKey(position))
                {
                    Neighbors[position] = 1;
                }
                else Neighbors[position]++;
            }
        }
    }
}
