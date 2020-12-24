using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14
{
    static class Program
    {
        static void Main(string[] args)
        {
            string[] text = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day14/input.txt");
            Console.WriteLine("Sum = " + GetSum(text));
            Console.WriteLine("Sum 2 = " + GetSumRedux(text));
        }

        static long GetSumRedux(string[] text)
        {
            Dictionary<long, long> memory = new Dictionary<long, long>();
            string currentMask = string.Empty;
            text.ToList().ForEach(line =>
            {
                string[] lineSplit = line.Split('=');
                if (line.StartsWith("mas")) currentMask = lineSplit[1].Trim();
                else
                {
                    char[] reverseBinary = ToBinaryString(long.Parse(SubstringBetween(lineSplit[0], "[", "]")), currentMask.Length)
                                                .Select((item, i) => currentMask[i] == '1' || currentMask[i] == 'X' ? currentMask[i] : item)
                                                .Reverse()
                                                .ToArray();

                    List<int> floatingIndexes = reverseBinary.Select((item, index) => new { item, index }).Where(x => x.item == 'X').Select(x => x.index).ToList();

                    for (long i = 0; i < Math.Pow(2, floatingIndexes.Count); i++)
                    {
                        string currentCombo = ToBinaryString(i, floatingIndexes.Count);
                        for (int j = 0; j < currentCombo.Length; j++)
                        {
                            reverseBinary[floatingIndexes[j]] = currentCombo[j];
                        }

                        memory[new string(reverseBinary).ReverseString().BinaryToNumber()] = long.Parse(lineSplit[1]);
                    }
                }
            });
            return memory.Sum(x => x.Value);
        }

        public static string ReverseString(this string text)
        {
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }

        static long GetSum(string[] text)
        {
            Dictionary<long, long> memory = new Dictionary<long, long>();
            string currentMask = string.Empty;
            text.ToList().ForEach(line =>
            {
                string[] lineSplit = line.Split('=');
                if (line.StartsWith("mas")) currentMask = lineSplit[1].Trim();
                else
                {
                    IEnumerable<char> binaryNumbers = ToBinaryString(long.Parse(lineSplit[1]), currentMask.Length)
                                       .Select((item, index) => currentMask[index] != 'X' ? currentMask[index] : item);

                    memory[long.Parse(SubstringBetween(lineSplit[0], "[", "]"))] =
                               BinaryToNumber(binaryNumbers);
                }
            });
            return memory.Sum(x => x.Value);
        }

        public static long BinaryToNumber(this IEnumerable<char> binaryString)
        {
            return BinaryToNumber(new string(binaryString.ToArray()));
        }

        static long BinaryToNumber(this string binaryString)
        {
            return Convert.ToInt64(binaryString, 2);
        }

        static string ToBinaryString(long number, int length)
        {
            // return Enumerable.Range(0, (int) Math.Log(number, 2) + 1).Aggregate(string.Empty, (collected, bitshift) => ((number >> bitshift) & 1 )+ collected);
            string bin = Convert.ToString(number, 2);
            string result = string.Empty;
            for (int i = 0; i < length - bin.Length; i++)
            {
                result += "0";
            }

            result += bin;
            return result;
        }

        static string SubstringBetween(string text, string leftString, string rightString)
        {
            int leftIndex = text.IndexOf(leftString);
            int rightIndex = text.IndexOf(rightString);
            if (leftIndex != -1 && rightIndex != -1 && leftIndex < rightIndex)
            {
                return text.Substring(leftIndex + 1, rightIndex - leftIndex - 1);
            }

            return string.Empty;
        }
    }
}
