using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        static long invalidNumber = 0;
        static int invalidNumberLocation = 0;

        static void Main(string[] args)
        {
            string[] numbers = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day9/input.txt").Split('\n');
            Console.WriteLine("Part1: " + Part1(numbers));
            Console.WriteLine("Part2: " + Part2(numbers));
        }

        static long Part1(string[] numbers)
        { 
            long value = 0;

            HashSet<long> validNumbers = new HashSet<long>();
            int preamble = 25;
            int endPtr = preamble;
            int startPtr = 0;

            while (endPtr < numbers.Length)
            {
                for (int i = startPtr; i < (preamble+startPtr); i++)
                {
                    validNumbers.Add(long.Parse(numbers[i]));
                }

                bool isValid = false;
                for (int i = startPtr; i < (preamble+startPtr); i++)
                {
                    long number1 = long.Parse(numbers[i]);
                    long sum = long.Parse(numbers[endPtr]);
                    if (validNumbers.Contains(sum - number1))
                    {
                        isValid = true;
                        break;
                    }
                }
                if (!isValid) 
                {
                    value = long.Parse(numbers[endPtr]);
                    break;
                } 
                validNumbers.Clear();
                startPtr++;
                endPtr++;
            }

            invalidNumber = value;
            invalidNumberLocation = endPtr;

            return value;
        }

        static long Part2(string[] list)
        {
            List<long> numbers = new List<long>();
            for (int i = 0; i < invalidNumberLocation; i++)
            {
                numbers.Add(long.Parse(list[i]));
            }

            //numbers.Sort();

            int startPtr = 0;
            int endPtr = 1;
            List<long> usedNumbers = new List<long>();

            long sum = 0;
            while (startPtr < numbers.Count - 1)
            {
                if (sum == invalidNumber)
                {
                    break;
                }

                sum = numbers[startPtr] + numbers[endPtr];
                usedNumbers.Add(numbers[startPtr]);
                usedNumbers.Add(numbers[endPtr]);  

                while (endPtr < numbers.Count)
                {
                    if (sum == invalidNumber)
                    {
                        break;
                    }
                    else if (sum < invalidNumber)
                    {
                        endPtr++;
                        sum += numbers[endPtr];
                        usedNumbers.Add(numbers[endPtr]);
                    }
                    else // sum > invalidNumber
                    {
                        startPtr++;
                        endPtr = startPtr + 1;
                        usedNumbers.Clear();
                        sum = 0;
                        break;
                    }
                }
            }

            long weakness = usedNumbers.Min() + usedNumbers.Max();

            return weakness;
        }
    }
}
