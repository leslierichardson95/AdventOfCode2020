using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Schema;

namespace Day22
{
    class Program
    {

        static void Main(string[] args)
        {
            string text = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day22/input.txt");
            text = text.Replace("Player 1:\n", "");
            text = text.Replace("Player 2:\n", "");

            string[] playerInputs = text.Split("\n\n");

            Console.WriteLine($"Winner Score: {GetWinnerScore(playerInputs, true)}");
        }

        static int GetWinnerScore(string[] playerInputs, bool isPart2)
        {
            (Queue<int> PlayerDeck1, Queue<int> PlayerDeck2) = ParseInput(playerInputs);
            Queue<int> winningDeck;

            if (!isPart2)
            {
                winningDeck = PlayGame(PlayerDeck1, PlayerDeck2);
            }
            else
            {
                (winningDeck, _) = PlayRecursiveCombat(PlayerDeck1, PlayerDeck2);
            }

            int totalScore = 0;
            int position = winningDeck.Count;

            int card = winningDeck.Dequeue();
            while (winningDeck.Count != 0)
            {
                totalScore += card * position;
                position--;
                card = winningDeck.Dequeue();
            }
            totalScore += card * position; // calculate final card
            return totalScore;
        }

        static (Queue<int>, int) PlayRecursiveCombat(Queue<int> PlayerDeck1, Queue<int> PlayerDeck2)
        {
            List<List<int>> p1History = new List<List<int>>();
            List<List<int>> p2History = new List<List<int>>();

            while (PlayerDeck1.Count != 0 && PlayerDeck2.Count != 0)
            {
                List<int> p1Deck = PlayerDeck1.ToList();
                List<int> p2Deck = PlayerDeck2.ToList();

                if (p1History.Any(x => x.SequenceEqual(p1Deck)) || p2History.Any(x => x.SequenceEqual(p2Deck)))
                { 
                    return (PlayerDeck1, 1); 
                } 
                else
                {
                    p1History.Add(p1Deck);
                    p2History.Add(p2Deck);

                    int card1 = PlayerDeck1.Dequeue();
                    int card2 = PlayerDeck2.Dequeue();

                    if (PlayerDeck1.Count >= card1 && PlayerDeck2.Count >= card2)
                    {
                        Queue<int> newP1Deck = new Queue<int>();
                        Queue<int> newP2Deck = new Queue<int>();

                        for (int i = 1; i <= card1; i++)
                        {
                            newP1Deck.Enqueue(p1Deck[i]);
                        }
                        for (int i = 1; i <= card2; i++)
                        {
                            newP2Deck.Enqueue(p2Deck[i]);
                        }

                        (Queue<int> winnerDeck, int winner) = PlayRecursiveCombat(newP1Deck, newP2Deck);
                        if (winner == 1)
                        {
                            PlayerDeck1.Enqueue(card1);
                            PlayerDeck1.Enqueue(card2);
                        }
                        else if (winner == 2)
                        {
                            PlayerDeck2.Enqueue(card2);
                            PlayerDeck2.Enqueue(card1);
                        }
                    }
                    else
                    {
                        if (card1 > card2)
                        {
                            PlayerDeck1.Enqueue(card1);
                            PlayerDeck1.Enqueue(card2);
                        }
                        else if (card2 > card1)
                        {
                            PlayerDeck2.Enqueue(card2);
                            PlayerDeck2.Enqueue(card1);
                        }
                    }
                }
            }

            if (PlayerDeck1.Count == 0) return (PlayerDeck2, 2);
            else return (PlayerDeck1, 1);
        }

        static Queue<int> PlayGame(Queue<int> PlayerDeck1, Queue<int> PlayerDeck2)
        {
            while (PlayerDeck1.Count != 0 && PlayerDeck2.Count != 0)
            {
                int card1 = PlayerDeck1.Dequeue();
                int card2 = PlayerDeck2.Dequeue();

                if (card1 > card2)
                {
                    PlayerDeck1.Enqueue(card1);
                    PlayerDeck1.Enqueue(card2);
                }
                else if (card2 > card1)
                {
                    PlayerDeck2.Enqueue(card2);
                    PlayerDeck2.Enqueue(card1);
                }
            }

            if (PlayerDeck1.Count == 0) return PlayerDeck2;
            else return PlayerDeck1;
        }

        static (Queue<int>, Queue<int>) ParseInput(string[] playerInputs)
        {
            Queue<int> PlayerDeck1 = new Queue<int>();
            Queue<int> PlayerDeck2 = new Queue<int>();

            string[] deck1 = playerInputs[0].Split('\n');
            string[] deck2 = playerInputs[1].Split('\n');

            for (int i = 0; i < deck1.Length; i++)
            {
                PlayerDeck1.Enqueue(int.Parse(deck1[i]));
                PlayerDeck2.Enqueue(int.Parse(deck2[i]));
            }
            return (PlayerDeck1, PlayerDeck2);
        }
    }
}