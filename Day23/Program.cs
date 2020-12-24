using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Day23
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedCups cupsPt1 = new LinkedCups("327465189", false);
            //LinkedCups cupsPt2 = new LinkedCups("327465189", true);

            //LinkedCups testCups = new LinkedCups("389125467");
            Console.WriteLine($"Cup Labels: {GetCupLabels(cupsPt1, 100, false)}");
            //Console.WriteLine($"Cup Labels: {GetCupLabels(cupsPt2, 1_000_000, true)}");
        }

        static string GetCupLabels(LinkedCups cups, int totalMoves, bool isPart2)
        {
            LinkedCupNode currentCup = cups.Head;

            for (int i = 0; i < totalMoves; i++)
            {
                int currentCupLabel = currentCup.CupLabel;
                LinkedCups pickedUpCups = cups.PickUpCups(currentCup);

                LinkedCupNode destinationCup = cups.GetDestinationCup(currentCupLabel);
                cups.InsertPickedUpCups(pickedUpCups, destinationCup);

                currentCup = currentCup.NextCup;
            }
            return cups.ToString();
        }

        class LinkedCups
        {
            public LinkedCupNode Head;
            public int MaxCupLabel = 0;
            public int MinCupLabel = 0;
            public Dictionary<int, LinkedCupNode> cupNodes;

            public LinkedCups(string cups, bool isPart2)
            {
                Head = new LinkedCupNode(int.Parse(cups[0].ToString()));
                LinkedCupNode cupNode = Head;

                for (int i = 1; i < cups.Length; i++)
                {
                    int label = int.Parse(cups[i].ToString());
                    cupNode.NextCup = new LinkedCupNode(label);
                    cupNodes[label] = cupNode.NextCup;
                    cupNode = cupNode.NextCup;
                }
                CalculateMaxMinCupLabel();

                if (isPart2)
                {
                    for (int i = MaxCupLabel+1; i < 1_000_000; i++)
                    {
                        cupNode.NextCup = new LinkedCupNode(i);
                        cupNodes[i] = cupNode.NextCup;
                        cupNode = cupNode.NextCup;
                    }
                }
                cupNode.NextCup = Head; // loop back to start
            }

            public LinkedCups() { }

            // Create non-cyclical list
            public LinkedCups(int cup)
            {
                Head = new LinkedCupNode(cup);
            }

            public LinkedCups PickUpCups(LinkedCupNode currentCup)
            {
                LinkedCupNode pickedUpCup = new LinkedCupNode(currentCup.NextCup.CupLabel); // first cup to be removed
                LinkedCups pickedUpCups = new LinkedCups();

                LinkedCupNode head = currentCup;
                for (int i = 0; i < 3; i++)
                {
                    RemoveCup(pickedUpCup);
                    pickedUpCups.AddCup(pickedUpCup.CupLabel);

                    head = head.NextCup;
                    pickedUpCup = head;
                }
                CalculateMaxMinCupLabel();

                return pickedUpCups;
            }

            public LinkedCupNode GetDestinationCup(int cupLabel)
            {
                cupLabel--;

                LinkedCupNode head = Head;
                int start = head.CupLabel;

                while (cupLabel != head.CupLabel)
                {
                    //if (cupLabel == head.CupLabel) return head;

                    if (cupLabel < MinCupLabel) cupLabel = MaxCupLabel;
                    if (cupLabel == head.CupLabel) break;

                    if (head.NextCup.CupLabel == start) cupLabel--;

                    head = head.NextCup;
                }

                return head;
            }

            public void InsertPickedUpCups(LinkedCups pickedUpCups, LinkedCupNode destinationCup)
            {
                LinkedCupNode head = Head;

                if (head == null) return;
                while (head.CupLabel != destinationCup.CupLabel) head = head.NextCup;

                LinkedCupNode next = head.NextCup;
                head.NextCup = pickedUpCups.Head;
                head = head.NextCup;

                while (head.NextCup != null) head = head.NextCup;
                head.NextCup = next;
            }

            public override string ToString()
            {
                string str = "";
                LinkedCupNode head = Head;

                while (head.CupLabel != 1) head = head.NextCup;

                int start = head.CupLabel;
                while (head.NextCup.CupLabel != start)
                {
                    str += $"{head.NextCup.CupLabel}";
                    head = head.NextCup;
                }
                return str;
            }

            public void CalculateMaxMinCupLabel()
            {
                MaxCupLabel = Head.CupLabel;
                MinCupLabel = Head.CupLabel;

                LinkedCupNode head = Head;
                int start = head.CupLabel;
                
                if (head.NextCup != null)
                {
                    if (head.CupLabel > MaxCupLabel) MaxCupLabel = head.CupLabel;
                    else if (head.CupLabel < MinCupLabel) MinCupLabel = head.CupLabel;
                    return;
                }
                while (head.NextCup.CupLabel != start)
                {
                    if (head.CupLabel > MaxCupLabel) MaxCupLabel = head.CupLabel;
                    else if (head.CupLabel < MinCupLabel) MinCupLabel = head.CupLabel;
                    head = head.NextCup;
                }
                if (head.CupLabel > MaxCupLabel) MaxCupLabel = head.CupLabel;
                else if (head.CupLabel < MinCupLabel) MinCupLabel = head.CupLabel;
            }


            // Add cup to end of the non-cyclical list
            public void AddCup(int cup)
            {
                LinkedCupNode head = Head;
                if (head == null)
                {
                    Head = new LinkedCupNode(cup);
                    return;
                }

                while (head.NextCup != null) head = head.NextCup;
                head.NextCup = new LinkedCupNode(cup);

            }

            public void RemoveCup(LinkedCupNode cup)
            {
                LinkedCupNode head = Head;
                LinkedCupNode previous = null;

                if (head == null) return;

                if (head.CupLabel == cup.CupLabel)
                {
                    Head = head.NextCup;
                    LinkedCupNode newHead = Head;

                    while (newHead.NextCup.CupLabel != cup.CupLabel) newHead = newHead.NextCup;
                    newHead.NextCup = Head;

                    return;
                }
                while (head.CupLabel != cup.CupLabel)
                {
                    previous = head;
                    head = head.NextCup;
                }

                previous.NextCup = head.NextCup;
            }

        }

        class LinkedCupNode
        {
            public int CupLabel;
            public LinkedCupNode NextCup;

            public LinkedCupNode(int cup)
            {
                CupLabel = cup;
                NextCup = null;
            }
        }
    }

    internal class Node
    {
        public int Val { get; set; }
        public Node Next { get; set; }

        public Node(int v)
        {
            Val = v;
        }
    }
}
