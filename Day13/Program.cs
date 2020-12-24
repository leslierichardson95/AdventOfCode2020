using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> text = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day13/input.txt").Split('\n', 'x', ',').ToList();
            text.RemoveAll(x => x.Equals(""));

            List<int> notes = new List<int>();
            for (int i = 0; i < text.Count; i++) 
            {
                notes.Add(int.Parse(text[i]));
            }
            List<string> text2 = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day13/input.txt").ToList();

            Console.WriteLine("Bus ID Product: " + GetIdProduct(notes));
            Console.WriteLine("Earliest Timestamp: " + GetTimestamp(text2));
        }

        public static long GetTimestamp(List<string> text)
        {
            List<long> departures = text[1]
                .Split(",")
                .Select(x =>
                {
                    if (long.TryParse(x, out var e))
                    {
                        return e;
                    }

                    return 0;
                })
                .ToList();

            long answer = ChineseRemainderTheorem(
                departures
                    .Where(x => x > 0)
                    .Select(x => x)
                    .ToArray(),
                departures
                    .Select((x, i) => new { i, x })
                    .Where(x => x.x > 0)
                    .Select(x => (x.x - x.i) % x.x) //(Bus ID - Position) % Bus ID
                    .ToArray()
                );
            return answer;
        }

        private static long ChineseRemainderTheorem(long[] n, long[] a)
        {
            long product = n.Aggregate(1, (long i, long j) => i * j);
            long sum = 0;

            for (int i = 0; i < n.Length; i++)
            {
                long p = product / n[i];

                sum += a[i] * ModMultInverse(p, n[i]) * p;
            }

            return sum % product;
        }

        static long ModMultInverse(long a, long mod)
        {
            long b = a % mod;

            for (int x = 1; x < mod; x++)
            {
                if ((b * x) % mod == 1)
                {
                    return x;
                }
            }

            return 1;
        }

        static long GetIdProduct(List<int> notes)
        {
            long firstTimestamp = notes[0];
            int busId = 0;

            long timestamp = firstTimestamp;
            bool isBusNotFound = true;
            while (isBusNotFound)
            {
                for (int i = 1; i < notes.Count; i++)
                {
                    if (timestamp % notes[i] == 0)
                    {
                        busId = notes[i];
                        isBusNotFound = false;
                        break;
                    }
                }
                if (isBusNotFound) timestamp++;
            }

            long product = (timestamp - firstTimestamp) * busId;
            return product;
        }
    }
}
