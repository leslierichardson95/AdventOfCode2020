using System;
using System.Collections.Generic;
using System.IO;

namespace Day5
{
    class Program
    {
        static HashSet<int> seatIds;
        static void Main(string[] args)
        {
            seatIds = new HashSet<int>();
            int maxSeatId = HighestSeatId();
            Console.WriteLine("Highest Seat Id: " + maxSeatId);
            Console.WriteLine("My Seat Id: " + MySeatId(maxSeatId));
        }

        static int HighestSeatId()
        {
            StreamReader sr = new StreamReader("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day5/input.txt");
            string[] passes = sr.ReadToEnd().Split('\n');

            int maxSeatId = 0;

            foreach (string pass in passes)
            {
                int lowRow = 0;
                int highRow = 127;
                int medRow = 0;

                for (int i = 0; i < 7; i++)
                {
                    char letter = pass[i];
                    medRow = (lowRow + highRow) / 2;

                    if (letter == 'F')
                    {
                        highRow = medRow;
                    }
                    else if (letter == 'B')
                    {
                        lowRow = medRow + 1;
                    }
                }
                medRow = (lowRow + highRow) / 2;

                int lowCol = 0;
                int highCol = 7;
                int medCol = 0;

                for (int i = 7; i < 10; i++)
                {
                    char letter = pass[i];
                    medCol = (lowCol + highCol) / 2;

                    if (letter == 'L')
                    {
                        highCol = medCol;
                    }
                    else if (letter == 'R')
                    {
                        lowCol = medCol + 1;
                    }
                }
                medCol = (lowCol + highCol) / 2;

                Console.WriteLine("Seat: Row " + medRow + ", Col " + medCol);

                int seatId = (medRow * 8) + medCol;
                if (seatId > maxSeatId) maxSeatId = seatId;

                seatIds.Add(seatId);
            }

            return maxSeatId;
        }

        static int MySeatId(int maxSeatId)
        {
            int mySeatId = 0;

            for (int i = 0; i < maxSeatId; i++)
            {
                if (!seatIds.Contains(i))
                {
                    if (seatIds.Contains(i-1) && seatIds.Contains(i+1))
                    {
                        mySeatId = i;
                        break;
                    }
                }
            }

            return mySeatId;
        }
    }
}
