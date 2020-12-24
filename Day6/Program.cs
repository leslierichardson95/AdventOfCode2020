using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sum for Anyone: " + Part1());
            Console.WriteLine("Sum for Everyone: " + Part2());
        }

        static int Part1()
        {
            string[] groups = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day6/input.txt").Split("\n\n");
            int sum = 0;

            foreach (string group in groups)
            {
                string trimmedGroup = group.Replace("\n", "");
                sum += trimmedGroup.Distinct<char>().Count();
            }

            return sum;
        }

        static int Part2()
        {
            string[] groups = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day6/input.txt").Split("\n\n");
            int sum = groups.Sum(group =>
            {
                Dictionary<char, int> letterCounts = group.ToCharArray()
                    .GroupBy(ch => ch)
                    .ToDictionary(g => g.Key, g => g.ToList().Count);

                var group_size = letterCounts.ContainsKey('\n') ? letterCounts['\n'] + 1 : 1;
                return letterCounts.Keys.Select(k => letterCounts[k]).Where(v => v == group_size).Count();
            });

            return sum;
        }
    }
}
