using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day24
{
    class Program
    {
        // using hexagon coordinates
        static readonly Dictionary<string, (int, int)> Directions = new Dictionary<string, (int, int)>()
        {
            { "e", (1,0) }, {"se", (0,1) }, {"sw", (-1,1) }, {"w", (-1,0) }, {"nw", (0,-1) }, {"ne", (1,-1) }
        };

        //True = black, False = white
        static Dictionary<(int, int), bool> FoundTiles = new Dictionary<(int, int), bool>();

        static void Main(string[] args)
        {
            string[] tiles = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day24/input.txt");
            Console.WriteLine($"Black Tiles: {FlipTiles(tiles)}");
            Console.WriteLine($"Black Tiles Pt2: {FlipTilesPt2()}");
        }

        static int FlipTilesPt2()
        {
            // filter out white tiles
            foreach (var foundTile in FoundTiles.Where(x => !x.Value)) FoundTiles.Remove(foundTile.Key);

            for (int i = 0; i < 100; i++)
            {
                foreach ((int, int) tile in FoundTiles.Keys.ToList())
                {
                    foreach ((int, int) direction in Directions.Values) // check neighbors for black/white
                    {
                        (int, int) neighborTile = (tile.Item1 + direction.Item1, tile.Item2 + direction.Item2);
                        if (!FoundTiles.ContainsKey(neighborTile)) FoundTiles[neighborTile] = false;
                    }
                }

                Dictionary<(int, int), bool> newTiles = new Dictionary<(int q, int r), bool>(FoundTiles);
                List<(int,int)> currentTiles = FoundTiles.Keys.ToList();
                foreach ((int,int) tile in currentTiles)
                {
                    int blackTiles = 0;
                    foreach ((int,int) direction in Directions.Values)
                    {
                        (int, int) neighborTile = (tile.Item1 + direction.Item1, tile.Item2 + direction.Item2);
                        bool tileColor = false;
                        if (FoundTiles.ContainsKey(neighborTile)) tileColor = FoundTiles[neighborTile];

                        if (tileColor) blackTiles++;
                    }

                    if (FoundTiles[tile])
                    {
                        newTiles[tile] = !(blackTiles == 0 || blackTiles > 2);
                    }
                    else
                    {
                        newTiles[tile] = (blackTiles == 2);
                    }
                }


                FoundTiles = new Dictionary<(int, int), bool>(newTiles);
                foreach (var foundTile in FoundTiles.Where(kvp => !kvp.Value).ToList()) FoundTiles.Remove(foundTile.Key); // filter out white tiles
            }

            return FoundTiles.Count(x => x.Value);
        }

        static int FlipTiles(string[] tiles)
        {
            foreach (string tile in tiles)
            {
                int i = 0;
                (int x, int y) position = (0, 0); // reference tile location

                while (i < tile.Length)
                {
                    if (tile[i] == 'e')
                    {
                        position.x += Directions["e"].Item1;
                        position.y += Directions["e"].Item2;
                        i++;
                    }
                    else if (tile[i] == 'w')
                    {
                        position.x += Directions["w"].Item1;
                        position.y += Directions["w"].Item2;
                        i++;
                    }
                    else if (tile[i] == 's')
                    {
                        i++;
                        if (tile[i] == 'w')
                        {
                            position.x += Directions["sw"].Item1;
                            position.y += Directions["sw"].Item2;
                            i++;
                        }
                        else if (tile[i] == 'e')
                        {
                            position.x += Directions["se"].Item1;
                            position.y += Directions["se"].Item2;
                            i++;
                        }
                    }
                    else if (tile[i] == 'n')
                    {
                        i++;
                        if (tile[i] == 'w')
                        {
                            position.x += Directions["nw"].Item1;
                            position.y += Directions["nw"].Item2;
                            i++;
                        }
                        else if (tile[i] == 'e')
                        {
                            position.x += Directions["ne"].Item1;
                            position.y += Directions["ne"].Item2;
                            i++;
                        }
                    }
                }
                if (FoundTiles.ContainsKey(position))
                {
                    FoundTiles[position] = !FoundTiles[position];
                }
                else FoundTiles[position] = true;
            }

            return FoundTiles.Count(x => x.Value);
        }
    }
}
