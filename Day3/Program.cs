using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Day3
{
    class Program
    {
        private static List<List<char>> map; 
        static void Main(string[] args)
        {
            long product = CountTrees(1, 1) * CountTrees(3, 1) * CountTrees(5, 1) * CountTrees(7, 1) * CountTrees(1, 2);
            Console.WriteLine("Product: " + product);
            //Console.WriteLine("Right 1, Down 1: " + CountTreesRedux(1,1));
            //Console.WriteLine("Right 3, Down 1: " + CountTreesRedux(3,1));
            //Console.WriteLine("Right 5, Down 1: " + CountTreesRedux(5,1));
            //Console.WriteLine("Right 7, Down 1: " + CountTreesRedux(7,1));
            //Console.WriteLine("Right 1, Down 2: " + CountTreesRedux(1,2));
            //Console.WriteLine("hi");
        }

        static void AppendMap(List<List<char>> map)
        {
            int lineCount = map[0].Count * 2;
            for (int x = 0; x < map.Count; x++)
            {
                int i = 0;
                
                for (int y = map[x].Count; y < lineCount; y++)
                {
                    map[x].Add(map[x][i]);
                    i++;
                }
            }

        }

        static long CountTrees(int rightTraversal, int downTraversal)
        {
            map = new List<List<char>>();

            StreamReader sr = new StreamReader("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day3/input.txt");
            string line = sr.ReadLine();

            // Load Initial Map
            while (line != null)
            {
                List<char> x = new List<char>();
                for (int y = 0; y < line.Length; y++)
                {
                    x.Add(line[y]);                 
                }
                map.Add(x);
                line = sr.ReadLine();
            }

            long trees = 0;
            int row = downTraversal;  
            int col = 0;

            while (row < map.Count)
            {
                //while (col < map[row].Count)
                //{
                    if (col + rightTraversal >= map[row].Count)
                    {
                        AppendMap(map);
                        if (map[row][col + rightTraversal] == '#')
                        {
                            trees++;
                        } 
                    }
                    else 
                    {
                        if (map[row][col + rightTraversal] == '#')
                        {
                            trees++;
                        }
                    }
                    col += rightTraversal;
                //}

                row += downTraversal;
            }

            return trees;
        }

        static long CountTreesRedux(int right, int down)
        {
            map = new List<List<char>>();

            StreamReader sr = new StreamReader("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day3/input.txt");
            string line = sr.ReadLine();

            // Load Initial Map
            while (line != null)
            {
                List<char> x = new List<char>();
                for (int y = 0; y < line.Length; y++)
                {
                    x.Add(line[y]);
                }
                map.Add(x);
                line = sr.ReadLine();
            }

            long treeCount = 0;
            for (int i = 0, j = 0; i < map.Count-1;)
            {
                j += right;
                i += down;
                if (j >= map[0].Count)
                    j = j % map[0].Count;
                if (map[i][j] == '#')
                    treeCount++;
            }
            return treeCount;
        }
    }
}
