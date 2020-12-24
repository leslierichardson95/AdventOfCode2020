using System;
using System.Collections.Generic;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            FileReader fr = new FileReader("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day1/input.txt");
            List<int> expenses = fr.ConvertToIntArray();
            //Console.WriteLine("Pair Product: " + GetSumPairsProduct(expenses, 2020));
            //List<int> test = new List<int> { 1721, 979, 366, 299, 675, 1456 };
            Console.WriteLine("Triad Product: " + GetSumTriadProduct(expenses, 2020));
        }

        static int GetSumPairsProduct(List<int> expenses, int sum)
        {
            HashSet<int> hs = new HashSet<int>();

            for (int i = 0; i < expenses.Count; i++)
            {
                int addend2 = sum - expenses[i];
                if (hs.Contains(addend2))
                {
                    Console.WriteLine("Sum Pair: " + expenses[i] + ", " + addend2);
                    return expenses[i] * addend2;
                }
                else
                {
                    hs.Add(expenses[i]);
                    
                }
            }
            return 0;
        }

        static int GetSumTriadProduct(List<int> expenses, int sum)
        {
            HashSet<int> hs = new HashSet<int>();

            for (int i = 0; i < expenses.Count; i++)
            {
                int addend1 = expenses[i];
                int sumPair = sum - expenses[i];

                if (!hs.Contains(addend1))
                {
                    hs.Add(addend1); // will check for duplicate
                }

                for (int j = i + 1; j < expenses.Count; j++)
                {
                    int addend2 = expenses[j];
                    int addend3 = sum - (addend1 + addend2);

                    if (hs.Contains(addend2) && hs.Contains(addend3)) {
                        Console.WriteLine("Sum Triad: " + addend1 + "," + addend2 + "," + addend3);
                        return addend1 * addend2 * addend3;
                    }
                    else
                    {
                        hs.Add(addend2);
                        //hs.Add(addend3);
                    }
                }
            }
            return 0;
        }
    }
}
