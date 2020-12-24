using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Bags with Gold Bag: " + Part1());
            Console.WriteLine("Total Bags for Gold Bag: " + Part2());
        }

        static int Part1()
        {

            Dictionary<string, string> bagRules = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day7/input.txt")
                .Select(str => str.Substring(0, str.Length - 1)
                .Replace(" bags", "").Replace(" bag", ""))
                .ToDictionary(str => str.Split(" contain ")[0], str => str.Split(" contain ")[1]);
            

            int bagCount = 0;
            foreach (string parentBag in bagRules.Keys)
            {
                if (ContainsShinyGold(bagRules, parentBag)) bagCount++;
            }

            return bagCount;
        }

        static int Part2()
        {
            Dictionary<string, string> bagRules = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day7/input.txt")
                .Select(str => str.Substring(0, str.Length - 1)
                .Replace(" bags", "").Replace(" bag", ""))
                .ToDictionary(str => str.Split(" contain ")[0], str => str.Split(" contain ")[1]);

            return GetTotalBagCount(bagRules, "shiny gold");
        }

        static int GetTotalBagCount(Dictionary<string, string> bagRules, string bag)
        {
            int bagTotal = 0;
            foreach (string childBag in bagRules[bag].Split(", "))
            {
                if (!childBag.Contains("no other"))
                {
                    int bagNumber = int.Parse(childBag.Substring(0, 1));
                    bagTotal += bagNumber + bagNumber * GetTotalBagCount(bagRules, childBag.Substring(2));
                }
                else break;
            }
            return bagTotal;
        }


        static bool ContainsShinyGold(Dictionary<string, string> bagRules, string parentBag)
        {
            if (bagRules[parentBag].Contains("shiny gold")) return true;
            else
            {
                foreach (string childBag in bagRules[parentBag].Split(", "))
                {
                    if (!childBag.Contains("no other"))
                    {
                        if (ContainsShinyGold(bagRules, childBag.Substring(2))) return true;
                    }
                }
            }
            return false;
        }
    }
}
