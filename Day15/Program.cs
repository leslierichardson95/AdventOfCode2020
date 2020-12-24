using System;
using System.Collections.Generic;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int>{ 14, 3, 1, 0, 9, 5 };
            //Console.WriteLine("2020th Turn: " + MemoryGameResult(numbers, 2020));
            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            Console.WriteLine("30000000th Turn: " + MemoryGameResult(numbers, 30000000));
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
        }

        static int MemoryGameResult(List<int> numbers, int lastTurn)
        {
            Dictionary<int, int> lastNumberIndexes = new Dictionary<int, int>();
            for (int i = 0; i < numbers.Count - 1; i++)
            {
                lastNumberIndexes.Add(numbers[i], i+1);
            }

            for (int turn = numbers.Count + 1; turn <= lastTurn; turn++)
            {
                int lastNumber = numbers[numbers.Count - 1];
                if (lastNumberIndexes.ContainsKey(lastNumber))
                {
                    int number = (turn - 1) - lastNumberIndexes[lastNumber];
                    if (number == 0) number = 1;

                    numbers.Add(number);
                    lastNumberIndexes[lastNumber] = turn - 1;
                }
                else 
                {
                    numbers.Add(0);
                    lastNumberIndexes.Add(lastNumber, turn - 1);
                } 
            }
            return numbers[numbers.Count - 1];
        }
    }
}
