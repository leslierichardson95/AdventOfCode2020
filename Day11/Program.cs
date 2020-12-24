using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day11
{
    class Program
    {
        public static Dictionary<(int, int), bool> Seats = new Dictionary<(int, int), bool>();
        public static Dictionary<(int, int), bool> Seats2;
        public static readonly List<(int, int)> Neighbors = new List<(int x, int y)>()
        {
            (1,0),
            (1,1),
            (0,1),
            (-1,1),
            (-1,0),
            (-1,-1),
            (0,-1),
            (1,-1)
        };
        static int maxX = 0;
        static int maxY = 0;

        static void Main(string[] args)
        {
            string[] text = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day11/input.txt");
            char[,] seatMap = new char[text.Length, text[0].Length];
            for (int i = 0; i < text.Length; i++)
            {
                for (int j = 0; j < text[0].Length; j++)
                {
                    seatMap[i, j] = text[i][j];
                }
            }
            Console.WriteLine("Occupied Seats: " + OccupiedSeats(seatMap));

            maxY = text.Length;
            for (int j = 0; j < text.Length; j++)
            {
                for (int i = 0; i < text[0].Length; i++)
                {
                    if (text[j][i] == 'L') Seats[(i, j)] = false;
                    else if (text[j][i] == '#') Seats[(i, j)] = false;
                    if (i > maxX) maxX = i;
                }
            }

            Seats2 = new Dictionary<(int, int), bool>(Seats);

            Console.WriteLine("Occupied Seats Pt2: " + OccupiedSeats2());

        }

        static string OccupiedSeats2()
        {
            Seats = new Dictionary<(int, int), bool>(Seats2);
            int seatsChanged = int.MaxValue;
            do
            {
                seatsChanged = 0;
                var nextSeats = new Dictionary<(int, int), bool>(Seats);
                foreach (var seat in Seats)
                {
                    bool nextVal = AliveNext(seat.Key, true);
                    if (nextVal != seat.Value) seatsChanged++;
                    nextSeats[seat.Key] = nextVal;
                }

                Seats = new Dictionary<(int, int), bool>(nextSeats);
            } while (seatsChanged != 0);


            return Seats.Count(x => x.Value).ToString();
        }
        //public static (int, int) Add(this (int x, int y) a, (int x, int y) b) => (a.x + b.x, a.y + b.y);

        static bool AliveNext((int x, int y) c, bool part2 = false)
        {
            int livingNeighbors = 0;
            List<(int, int)> locNeighbors = new List<(int x, int y)>();
            List<(int, int)> extendedNeighbors = new List<(int x, int y)>();
            foreach (var n in Neighbors)
            {
                (int x, int y) sum = (c.x + n.Item1, c.y + n.Item2);
                locNeighbors.Add(sum);

                var tmp = sum;
                while (!Seats.ContainsKey(tmp))
                {
                    if (tmp.Item1 < 0 || tmp.Item1 > maxX || tmp.Item2 < 0 || tmp.Item2 > maxY) break;
                    (int x, int y) sum2 = (tmp.x + n.Item1, tmp.y + n.Item2);
                    tmp = sum2;
                }

                extendedNeighbors.Add(tmp);
            }

            if (part2)
            {
                foreach (var n in extendedNeighbors)
                {
                    if (!Seats.ContainsKey(n)) continue;
                    if (Seats[n]) livingNeighbors++;
                }
                if (Seats[c])
                {
                    if (livingNeighbors < 5) return true;
                }
                else
                {
                    if (livingNeighbors == 0) return true;
                }
            }
            else
            {
                foreach (var n in locNeighbors)
                {
                    if (!Seats.ContainsKey(n)) continue;
                    if (Seats[n]) livingNeighbors++;
                }

                if (Seats[c])
                {
                    if (livingNeighbors < 4) return true;
                }
                else
                {
                    if (livingNeighbors == 0) return true;
                }
            }

            return false;
        }

        static int OccupiedSeats(char[,] seatMap)
        {
            int occupiedSeats = 0;
            bool seatsAreChanged = true;

            while (seatsAreChanged)
            {
                seatsAreChanged = false;

                char[,] newSeatMap = new char[seatMap.GetLength(0), seatMap.GetLength(1)];
                for (int row = 0; row < seatMap.GetLength(0); row++)
                {
                    for (int col = 0; col < seatMap.GetLength(1); col++)
                    {
                        if (seatMap[row, col] == 'L')
                        {
                            if (row == 0)
                            {
                                if (col == 0)
                                {
                                    if (seatMap[row + 1, col] != '#' && seatMap[row, col + 1] != '#' && seatMap[row + 1, col + 1] != '#')
                                    {
                                        newSeatMap[row, col] = '#';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = 'L';
                                }
                                else if (col == seatMap.GetLength(1) - 1)
                                {
                                    if (seatMap[row, col - 1] != '#' && seatMap[row + 1, col] != '#' && seatMap[row + 1, col - 1] != '#')
                                    {
                                        newSeatMap[row, col] = '#';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = 'L';
                                }
                                else
                                {
                                    if (seatMap[row + 1, col] != '#' && seatMap[row, col + 1] != '#' && seatMap[row, col - 1] != '#' &&
                                        seatMap[row + 1, col + 1] != '#' && seatMap[row + 1, col - 1] != '#')
                                    {
                                        newSeatMap[row, col] = '#';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = 'L';
                                }
                            }
                            else if (row == seatMap.GetLength(0) - 1)
                            {
                                if (col == 0)
                                {
                                    if (seatMap[row-1, col] != '#' && seatMap[row, col+1] != '#' && seatMap[row-1, col+1] != '#')
                                    {
                                        newSeatMap[row, col] = '#';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = 'L';
                                }
                                else if (col == seatMap.GetLength(1) - 1)
                                {
                                    if (seatMap[row-1, col] != '#' && seatMap[row, col-1] != '#' && seatMap[row-1, col-1] != '#')
                                    {
                                        newSeatMap[row, col] = '#';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = 'L';
                                }
                                else
                                {
                                    if (seatMap[row-1, col] != '#' && seatMap[row, col-1] != '#' && seatMap[row, col+1] != '#' &&
                                        seatMap[row-1, col+1] != '#' && seatMap[row-1, col-1] != '#')
                                    {
                                        newSeatMap[row, col] = '#';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = 'L';
                                }
                            }
                            else
                            {
                                if (col == 0)
                                {
                                    if (seatMap[row + 1, col] != '#' && seatMap[row - 1, col] != '#' && seatMap[row, col + 1] != '#' &&
                                        seatMap[row + 1, col + 1] != '#' && seatMap[row - 1, col + 1] != '#')
                                    {
                                        newSeatMap[row, col] = '#';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = 'L';
                                }
                                else if (col == seatMap.GetLength(1) - 1)
                                {
                                    if (seatMap[row + 1, col] != '#' && seatMap[row - 1, col] != '#' && seatMap[row, col - 1] != '#' &&
                                        seatMap[row + 1, col - 1] != '#' && seatMap[row - 1, col - 1] != '#')
                                    {
                                        newSeatMap[row, col] = '#';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = 'L';
                                }
                                else
                                {
                                    if (seatMap[row + 1, col] != '#' && seatMap[row - 1, col] != '#' && seatMap[row, col + 1] != '#' &&
                                    seatMap[row, col - 1] != '#' && seatMap[row + 1, col + 1] != '#' && seatMap[row - 1, col - 1] != '#' &&
                                    seatMap[row + 1, col - 1] != '#' && seatMap[row - 1, col + 1] != '#')
                                    {
                                        newSeatMap[row, col] = '#';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = 'L';
                                }
                            }
                        }
                        else if (seatMap[row, col] == '#')
                        {
                            int occupied = 0;
                            if (row == 0)
                            {
                                if (col == 0) newSeatMap[row, col] = '#';
                                else if (col == seatMap.GetLength(1) - 1) newSeatMap[row, col] = '#';
                                else
                                {
                                    if (seatMap[row + 1, col] == '#') occupied++;
                                    if (seatMap[row, col + 1] == '#') occupied++;
                                    if (seatMap[row, col - 1] == '#') occupied++;
                                    if (seatMap[row + 1, col + 1] == '#') occupied++;
                                    if (seatMap[row + 1, col - 1] == '#') occupied++;

                                    if (occupied >= 4) 
                                    {
                                        newSeatMap[row, col] = 'L';
                                        seatsAreChanged = true;
                                    } 
                                    else newSeatMap[row, col] = '#';
                                }
                            }
                            else if (row == seatMap.GetLength(0) - 1)
                            {
                                if (col == 0) newSeatMap[row, col] = '#';
                                else if (col == seatMap.GetLength(1) - 1) newSeatMap[row, col] = '#';
                                else
                                {
                                    if (seatMap[row - 1, col] == '#') occupied++;
                                    if (seatMap[row, col + 1] == '#') occupied++;
                                    if (seatMap[row, col - 1] == '#') occupied++;
                                    if (seatMap[row - 1, col + 1] == '#') occupied++;
                                    if (seatMap[row - 1, col - 1] == '#') occupied++;

                                    if (occupied >= 4) 
                                    {
                                        newSeatMap[row, col] = 'L';
                                        seatsAreChanged = true;
                                    } 
                                    else newSeatMap[row, col] = '#';
                                }
                            }
                            else
                            {
                                if (col == 0)
                                {
                                    if (seatMap[row + 1, col] == '#') occupied++;
                                    if (seatMap[row - 1, col] == '#') occupied++;
                                    if (seatMap[row, col + 1] == '#') occupied++;
                                    if (seatMap[row + 1, col + 1] == '#') occupied++;
                                    if (seatMap[row - 1, col + 1] == '#') occupied++;

                                    if (occupied >= 4)
                                    {
                                        newSeatMap[row, col] = 'L';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = '#';
                                }
                                else if (col == seatMap.GetLength(1) - 1)
                                {
                                    if (seatMap[row + 1, col] == '#') occupied++;
                                    if (seatMap[row - 1, col] == '#') occupied++;
                                    if (seatMap[row, col - 1] == '#') occupied++;
                                    if (seatMap[row + 1, col - 1] == '#') occupied++;
                                    if (seatMap[row - 1, col - 1] == '#') occupied++;

                                    if (occupied >= 4)
                                    {
                                        newSeatMap[row, col] = 'L';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = '#';
                                }
                                else
                                {
                                    if (seatMap[row + 1, col] == '#') occupied++;
                                    if (seatMap[row - 1, col] == '#') occupied++;
                                    if (seatMap[row, col + 1] == '#') occupied++;
                                    if (seatMap[row, col - 1] == '#') occupied++;
                                    if (seatMap[row + 1, col + 1] == '#') occupied++;
                                    if (seatMap[row - 1, col - 1] == '#') occupied++;
                                    if (seatMap[row + 1, col - 1] == '#') occupied++;
                                    if (seatMap[row - 1, col + 1] == '#') occupied++;

                                    if (occupied >= 4)
                                    {
                                        newSeatMap[row, col] = 'L';
                                        seatsAreChanged = true;
                                    }
                                    else newSeatMap[row, col] = '#';
                                }
                            }
                        }
                        else if (seatMap[row, col] == '.') newSeatMap[row, col] = '.';
                    }
                }
                seatMap = newSeatMap;
            }
            for (int i = 0; i < seatMap.GetLength(0); i++)
            {
                for (int j = 0; j < seatMap.GetLength(1); j++)
                {
                    if (seatMap[i, j] == '#') occupiedSeats++;
                }
            }

            return occupiedSeats;
        }
    }
}
