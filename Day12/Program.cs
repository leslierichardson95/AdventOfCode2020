using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] instructions = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day12/input.txt");
            Console.WriteLine("Manhattan Distance: " + GetDistance(instructions));
            Console.WriteLine("Manhattan Distance w/ Waypoint: " + GetDistanceRedux(instructions));
        }

        static int GetDistanceRedux(string[] instructions)
        {
            (int x, int y)[] directions = { (1, 1), (1, -1), (-1, -1), (-1, 1) };

            (int x, int y) shipPosition = (0, 0);
            (int x, int y) waypointPosition = (10, 1);
            int waypointQuad = 0;

            foreach (string instruction in instructions)
            {
                char action = instruction[0];
                int value = int.Parse(instruction.Substring(1));

                if (action == 'N')
                {
                    waypointPosition.y += value;
                }
                else if (action == 'S')
                {
                    waypointPosition.y -= value;
                }
                else if (action == 'E')
                {
                    waypointPosition.x += value;
                }
                else if (action == 'W')
                {
                    waypointPosition.x -= value;
                }
                else if (action == 'L')
                {
                    if (value == 90)
                    {
                        waypointPosition = (-waypointPosition.y, waypointPosition.x);
                    }
                    else if (value == 180)
                    {
                        waypointPosition = (-waypointPosition.x, -waypointPosition.y);
                    }
                    else if (value == 270)
                    {
                        waypointPosition = (waypointPosition.y, -waypointPosition.x);
                    }
                }
                else if (action == 'R')
                {
                    if (value == 90)
                    {
                        waypointPosition = (waypointPosition.y, -waypointPosition.x);
                    }
                    else if (value == 180)
                    {
                        waypointPosition = (-waypointPosition.x, -waypointPosition.y);
                    }
                    else if (value == 270)
                    {
                        waypointPosition = (-waypointPosition.y, waypointPosition.x);
                    }
                }
                else if (action == 'F')
                {
                    shipPosition.x += (value * waypointPosition.x);
                    shipPosition.y += (value * waypointPosition.y);
                }
            }

            int distance = Math.Abs(shipPosition.x) + Math.Abs(shipPosition.y);
            return distance;
        }

        static int GetDistance(string[] instructions)
        {
            char[] directions = { 'E', 'S', 'W', 'N' };

            (int x, int y) shipPosition = (0, 0);
            int shipOrientation = 0;

            foreach (string instruction in instructions)
            {
                char action = instruction[0];
                int value = int.Parse(instruction.Substring(1));

                if (action == 'N')
                {
                    shipPosition.y += value;
                }
                else if (action == 'S')
                {
                    shipPosition.y -= value;
                }
                else if (action == 'E')
                {
                    shipPosition.x += value;
                }
                else if (action == 'W')
                {
                    shipPosition.x -= value;
                }
                else if (action == 'L')
                {
                    int rotations = 4 - (value / 90);
                    shipOrientation = Math.Abs((shipOrientation + rotations) % directions.Length);
                }
                else if (action == 'R')
                {
                    int rotations = value / 90;
                    shipOrientation = Math.Abs((shipOrientation + rotations) % directions.Length);
                }
                else if (action == 'F')
                {
                    if (directions[shipOrientation] == 'N')
                    {
                        shipPosition.y += value;
                    }
                    else if (directions[shipOrientation] == 'S')
                    {
                        shipPosition.y -= value;
                    }
                    else if (directions[shipOrientation] == 'E')
                    {
                        shipPosition.x += value;
                    }
                    else if (directions[shipOrientation] == 'W')
                    {
                        shipPosition.x -= value;
                    }
                }
            }

            int distance = Math.Abs(shipPosition.x) + Math.Abs(shipPosition.y);
            return distance;
        }
    }
}
