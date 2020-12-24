using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] list = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day10/input.txt");
            List<int> adapterRatings = new List<int>();
            for (int i = 0; i < list.Length; i++)
            {
                adapterRatings.Add(int.Parse(list[i]));
            }
            int maxAdapterJolts = adapterRatings.Max();
            adapterRatings.Add(maxAdapterJolts + 3);
            adapterRatings.Sort();

            Console.WriteLine("Part1: " + Part1(adapterRatings));
            Console.WriteLine("Part2: " + Part2(list));
        }

        static int Part1(List<int> adapterRatings)
        {
            int joltDiff1 = 0;
            int joltDiff3 = 0;

            adapterRatings.Sort();

            int lastJolt = 0;
            for (int i = 0; i < adapterRatings.Count; i++)
            {
                int adapterRating = adapterRatings[i];
                if (adapterRating - lastJolt == 1) joltDiff1++;
                else if (adapterRating - lastJolt == 3) joltDiff3++;

                lastJolt = adapterRating;
            }
            
            return joltDiff1 * joltDiff3;
        }

        static long Part2(string[] list)
        {
            var adapterRatings = list.Select(x => int.Parse(x)).Append(0).OrderBy(x => x).ToArray();
            long[] steps = new long[adapterRatings.Length];
            steps[0] = 1;
            foreach (var i in Enumerable.Range(1, adapterRatings.Length - 1))
            {
                foreach (var j in Enumerable.Range(0, i))
                {
                    if (adapterRatings[i] - adapterRatings[j] <= 3)
                    {
                        steps[i] += steps[j];
                    }
                }
            }
            return steps.Last();
        }
    }
}
