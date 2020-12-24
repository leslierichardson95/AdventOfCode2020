using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day19/input.txt");
            Console.WriteLine($"Part 1: {GetValidMessgageCount(ParseInput(input), false)}");
            Console.WriteLine($"Part 2: {GetValidMessgageCount(ParseInput(input), true)}");
        }

        static int GetValidMessgageCount(Input input, bool isPart2)
        {
            var rules = input.Rules.ToList();
            if (isPart2)
            {
                rules.Add(new SequenceRule { Number = 8, Sequence = new() { 42, 8 } });
                rules.Add(new SequenceRule { Number = 11, Sequence = new() { 42, 11, 31 } });
            }
            var matcher = new Matcher(rules);
            int count = 0;
            foreach (var message in input.Messages)
            {
                if (matcher.IsMach(message))
                {
                    count++;
                }
            }
            return count;
        }

        static Input ParseInput(string input)
        {
            Input result = new Input();
            string[] lines = input.Trim().Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Regex regex = new Regex(@"^(\d+):(?: ""([^""]*)""|(?: ((?:\| )?)(\d+))+)\s*$");
            foreach (var line in lines)
            {
                if (regex.Match(line) is { Success: true } match)
                {
                    if (match.Groups[2].Success)
                    {
                        result.Rules.Add(new LiteralRule
                        {
                            Number = int.Parse(match.Groups[1].Value),
                            Literal = match.Groups[2].Value
                        });
                    }
                    else
                    {
                        List<int> sequence = new List<int>();
                        List<List<int>> alt = new List<List<int>> { sequence };
                        for (int i = 0; i < match.Groups[4].Captures.Count; i++)
                        {
                            if (match.Groups[3].Captures[i].Length != 0)
                            {
                                alt.Add(sequence = new List<int>());
                            }
                            sequence.Add(int.Parse(match.Groups[4].Captures[i].Value));
                        }
                        foreach (var seql in alt)
                        {
                            result.Rules.Add(new SequenceRule
                            {
                                Number = int.Parse(match.Groups[1].Value),
                                Sequence = seql
                            });
                        }
                    }
                }
                else
                {
                    result.Messages.Add(line);
                }
            }
            return result;
        }
    }

    class Matcher
    {
        private ILookup<int, Rule> _rules;

        public Matcher(IEnumerable<Rule> rules)
        {
            _rules = rules.ToLookup(r => r.Number);
        }

        public bool IsMach(string input)
        {
            foreach (var end in Match(input, 0, 0))
            {
                if (end == input.Length) return true;
            }
            return false;
        }

        IEnumerable<int> Match(string input, int number, int position)
        {
            foreach (var rule in _rules[number])
            {
                if (rule is LiteralRule literal)
                {
                    foreach (var end in MatchLit(input, literal, position))
                    {
                        yield return end;
                    }
                }
                else if (rule is SequenceRule sequence)
                {
                    foreach (var end in MatchSeq(input, sequence, position, 0))
                    {
                        yield return end;
                    }
                }
                else
                {
                    throw new ArgumentException(nameof(rule));
                }
            }
        }

        static IEnumerable<int> MatchLit(string input, LiteralRule lit, int pos)
        {
            if (string.CompareOrdinal(input, pos, lit.Literal, 0, lit.Literal.Length) == 0)
            {
                yield return pos + lit.Literal.Length;
            }
        }

        IEnumerable<int> MatchSeq(string input, SequenceRule sequence, int position, int index)
        {
            if (index == sequence.Sequence.Count)
            {
                yield return position;
                yield break;
            }
            foreach (var end in Match(input, sequence.Sequence[index], position))
            {
                foreach (var end2 in MatchSeq(input, sequence, end, index + 1))
                {
                    yield return end2;
                }
            }
        }
    }


    abstract class Rule
    {
        public int Number { get; set; }
    }
    class LiteralRule : Rule
    {
        public string Literal { get; set; }
    }
    class SequenceRule : Rule
    {
        public List<int> Sequence { get; set; }
    }
    class Input
    {
        public List<Rule> Rules { get; } = new List<Rule>();
        public List<string> Messages { get; } = new List<string>();
    }
}
