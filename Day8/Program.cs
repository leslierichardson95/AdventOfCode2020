using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Day8
{
    class Program
    {
        static int globalAcc = 0;
        static void Main(string[] args)
        {
            string[] instructions = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day8/input.txt").Split('\n');
            //Console.WriteLine("Accumulator: " + GetAccumulator(instructions));
            Console.WriteLine("Accumulator when Terminated: " + Part2(instructions));
        }
        static int Part2(string[] instructions)
        {
            for (int i = 0; i < instructions.Length; i++) { 
                if (instructions[i].Contains("jmp"))
                {
                    instructions[i] = instructions[i].Replace("jmp", "nop");
                    if (IsTerminated(instructions)) break;
                    else
                    {
                        globalAcc = 0;
                        instructions[i] = instructions[i].Replace("nop", "jmp");
                    }
                }
                else if (instructions[i].Contains("nop"))
                {
                    instructions[i] = instructions[i].Replace("nop", "jmp");
                    if (IsTerminated(instructions)) break;
                    else
                    {
                        globalAcc = 0;
                        instructions[i] = instructions[i].Replace("jmp", "nop");
                    }
                }
            }
            return globalAcc;
        }

        static bool IsTerminated(string[] instructions)
        {
            HashSet<int> usedInstructions = new HashSet<int>();
            int i = 0;
            while (!usedInstructions.Contains(i))
            {
                if (i >= instructions.Length) return true;

                string operation = instructions[i].Substring(0, 3);
                usedInstructions.Add(i);

                if (operation.Equals("acc"))
                {
                    int value = int.Parse(instructions[i].Substring(4));
                    globalAcc += value;
                    i++;
                }
                else if (operation.Equals("jmp"))
                {
                    int offset = int.Parse(instructions[i].Substring(4));
                    i += offset;
                }
                else if (operation.Equals("nop"))
                {
                    i++;
                    continue;
                }
            }

            return false;
        }

        static int GetAccumulator(string[] instructions)
        {
            int accumulator = 0;

            HashSet<int> usedInstructions = new HashSet<int>();
            int i = 0;
            while (!usedInstructions.Contains(i))
            {
                string operation = instructions[i].Substring(0, 3);
                usedInstructions.Add(i);

                if (operation.Equals("acc"))
                {
                    int value = int.Parse(instructions[i].Substring(4));
                    accumulator += value;
                    i++;
                }
                else if (operation.Equals("jmp"))
                {
                    int offset = int.Parse(instructions[i].Substring(4));
                    i += offset;
                }
                else if (operation.Equals("nop"))
                {
                    i++;
                    continue;
                }
            }

            return accumulator;
        }
    }
}
