using System;
using System.Collections.Generic;
using System.IO;

namespace Day25
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] input = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day25/input.txt");
            long[] publicKeys = new long[input.Length];
            for (int i = 0; i < publicKeys.Length; i++) publicKeys[i] = long.Parse(input[i]);

            Console.WriteLine($"Encryption Key: {GetEncryptionKey(publicKeys)}");
        }

        static long GetEncryptionKey(long[] publicKeys)
        {
            List<long> loopSizes = new List<long>();
            long subjectNumber = 7;
            long value = 1;
            long loopSize = 0;

            foreach (long publicKey in publicKeys)
            {
                while (value != publicKey)
                {
                    value *= subjectNumber;
                    value = value % 20201227;
                    loopSize++;
                }
                loopSizes.Add(loopSize);

                value = 1;
                loopSize = 0;
            }

            subjectNumber = publicKeys[0];
            loopSize = loopSizes[1];

            for (int i = 0; i < loopSize; i++)
            {
                value *= subjectNumber;
                value = value % 20201227;
            }

            return value;
        }
    }
}
