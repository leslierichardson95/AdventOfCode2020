using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day20
{
    class Program
    {
        static List<Tile> tiles;
        static void Main(string[] args)
        {
            string[] input = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day20/input.txt").Split("\n\n");
            ParseInput(input);
            Console.WriteLine($"Tile ID Product: {GetTileIdProduct()}");
        }

        static void ParseInput(string[] input)
        {
            tiles = new List<Tile>();

            foreach(string str in input)
            {
                string[] tile = str.Split(":\n");
                tile[0] = tile[0].Replace("Tile ", "");
                string[] contents = tile[1].Split('\n');
                tiles.Add(new Tile(long.Parse(tile[0]), contents));
            }
        }

        static long GetTileIdProduct()
        {
            long product = 0;
            SolvePuzzle();
            foreach (Tile tile in tiles)
            {
                if (tile.NullEdges == 2) product *= tile.Id;
            }
            return product;
        }

        static void SolvePuzzle()
        {
            foreach (Tile tile1 in tiles)
            {
                foreach (Tile tile2 in tiles)
                {
                    foreach (string edge in tile2.edges.Values)
                    {
                        if (tile1 == tile2) break;
                        if (tile1.edges.ContainsValue(edge))
                        {
                            if (tile2.IsRotated)
                            {
                                string edge1 = tile1.edges.FirstOrDefault(x => x.Value == edge).Key;
                                string matchedEdge = tile2.edges.FirstOrDefault(x => x.Value == edge).Key;
                                tile1.Rotate(matchedEdge, edge1);

                            }
                            else
                            {
                                string matchedEdge = tile1.edges.FirstOrDefault(x => x.Value == edge).Key;
                                string edge2 = tile2.edges.FirstOrDefault(x => x.Value == edge).Key;
                                tile2.Rotate(matchedEdge, edge2);
                            }                           
                        }
                    }
                }
            }
        }
    }

    class Tile
    {
        public long Id;
        public string[] FullContents;

        public Dictionary<string, string> edges;
        public Dictionary<string, Tile> adjacentTiles;

        public bool IsRotated;
        public int NullEdges;

        public Tile(long id, string[] contents)
        {
            Id = id;
            FullContents = contents;
            IsRotated = false;
            NullEdges = 4;

            adjacentTiles = new Dictionary<string, Tile>()
            {
                { "top", null }, {"bottom", null}, {"left", null}, {"right", null}
            };

            edges = new Dictionary<string, string>()
            {
                {"top", "" }, {"bottom", ""}, {"left", ""}, {"right", ""}
            };

            for (int i = 0; i < contents.Length; i++)
            {
                if (i == 0) edges["top"] = contents[i];
                else if (i == contents.Length - 1) edges["bottom"] = contents[i];

                edges["left"] += contents[i][0];
                edges["right"] += contents[i][contents.Length - 1];
            }
        }

        public bool Rotate(string matchedEdge, string currentEdge)
        {
            if ((matchedEdge.Equals("top") && currentEdge.Equals("bottom")) || 
                (currentEdge.Equals("top") && matchedEdge.Equals("bottom"))) return false;
            else if ((matchedEdge.Equals("right") && currentEdge.Equals("left")) ||
                (currentEdge.Equals("right") && matchedEdge.Equals("left"))) return false;
            else
            {
                if (currentEdge.Equals("top"))
                {
                    string temp = edges["top"];
                    edges["top"] = edges["bottom"];
                    edges["bottom"] = temp;

                    temp = edges["left"];
                    edges["left"] = edges["right"];
                    edges["right"] = temp;

                }
                else if (currentEdge.Equals("bottom"))
                {
                    string temp = edges["bottom"];
                    edges["bottom"] = edges["top"];
                    edges["top"] = temp;

                    temp = edges["left"];
                    edges["left"] = edges["right"];
                    edges["right"] = temp;
                }
                else if (currentEdge.Equals("left"))
                {
                    string temp = edges["left"];
                    edges["left"] = edges["right"];
                    edges["right"] = temp;

                    temp = edges["top"];
                    edges["top"] = edges["bottom"];
                    edges["bottom"] = temp;
                }
                else if (currentEdge.Equals("right"))
                {
                    string temp = edges["right"];
                    edges["right"] = edges["left"];
                    edges["left"] = temp;

                    temp = edges["top"];
                    edges["top"] = edges["bottom"];
                    edges["bottom"] = temp;
                }

                IsRotated = true;
                return true;
            }
        }

        public void ConnectTiles(Tile tile1, Tile tile2)
        {
            
        }
    }
}
