using System;
using System.Collections.Generic;

namespace Day23_Redux
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedCups cupsPt1 = new LinkedCups("327465189", false);
            LinkedCups cupsPt2 = new LinkedCups("327465189", true);

            LinkedCups testCups = new LinkedCups("389125467", true);
            //Console.WriteLine($"Cup Labels: {GetCupLabels(cupsPt1, 100, false)}");

            string[] twoCups = GetCupLabels(cupsPt2, 10_000_000, true).Split(" ");
            //Console.WriteLine($"Cup Labels: {GetCupLabels(cupsPt2, 1_000_000, true)}");
            Console.WriteLine($"Product: {long.Parse(twoCups[0]) * long.Parse(twoCups[1])}");
        }

        static string GetCupLabels(LinkedCups cups, int totalMoves, bool isPart2)
        {
            LinkedCupNode currentCup = cups.Head;

            for (int i = 0; i < totalMoves; i++)
            {
                int currentCupLabel = currentCup.CupLabel;
                LinkedCupNode firstPickedUpCup = cups.PickUpCups(currentCup);

                LinkedCupNode destinationCup = cups.GetDestinationCup(currentCupLabel, firstPickedUpCup);
                cups.InsertPickedUpCups(firstPickedUpCup, destinationCup);

                currentCup = currentCup.NextCup;
            }
            if (isPart2) return cups.ToString(true);
            else return cups.ToString();
        }

        class LinkedCups
        {
            public LinkedCupNode Head;
            public int MaxCupLabel = 0;
            public int MinCupLabel = 0;
            public Dictionary<int, LinkedCupNode> cupNodes;

            public LinkedCups(string cups, bool isPart2)
            {
                int firstLabel = int.Parse(cups[0].ToString());
                Head = new LinkedCupNode(firstLabel);

                cupNodes = new Dictionary<int, LinkedCupNode>();
                cupNodes[firstLabel] = Head;
                LinkedCupNode cupNode = Head;

                for (int i = 1; i < cups.Length; i++)
                {
                    int label = int.Parse(cups[i].ToString());
                    cupNode.NextCup = new LinkedCupNode(label);
                    cupNodes[label] = cupNode.NextCup;
                    cupNode = cupNode.NextCup;

                    if (label > MaxCupLabel) MaxCupLabel = label;
                    else if (label < MaxCupLabel) MinCupLabel = label;
                }
                //CalculateMaxMinCupLabel();

                if (isPart2)
                {
                    for (int i = MaxCupLabel + 1; i < 1_000_001; i++)
                    {
                        cupNode.NextCup = new LinkedCupNode(i);
                        cupNodes[i] = cupNode.NextCup;
                        cupNode = cupNode.NextCup;
                        MaxCupLabel = i;
                    }
                }
                cupNode.NextCup = Head; // loop back to start
            }

            public LinkedCupNode PickUpCups(LinkedCupNode currentCup)
            {
                LinkedCupNode pickedUpCup = currentCup.NextCup;
                currentCup.NextCup = pickedUpCup.NextCup.NextCup.NextCup;

                return pickedUpCup;
            }

            public LinkedCupNode GetDestinationCup(int cupLabel, LinkedCupNode firstPickedUpCup)
            {
                int destination = 0;
                if (cupLabel == 1) destination = MaxCupLabel;
                else destination = cupLabel - 1;

                int firstCup = firstPickedUpCup.CupLabel;
                int secondCup = firstPickedUpCup.NextCup.CupLabel;
                int thirdCup = firstPickedUpCup.NextCup.NextCup.CupLabel;

                while (destination == firstCup || destination == secondCup || destination == thirdCup)
                {
                    --destination;
                    if (destination <= 0)
                        destination = MaxCupLabel;
                }

                return cupNodes[destination];
            }

            public void InsertPickedUpCups(LinkedCupNode firstPickedUpCup, LinkedCupNode destinationCup)
            {
                LinkedCupNode head = Head;

                if (head == null) return;

                head = cupNodes[destinationCup.CupLabel];

                LinkedCupNode next = head.NextCup;
                LinkedCupNode tail = firstPickedUpCup.NextCup.NextCup;
                tail.NextCup = next;
                head.NextCup = firstPickedUpCup;
            }

            public string ToString(bool isPart2)
            {
                string str = "";
                LinkedCupNode head = cupNodes[1];

                if (isPart2)
                {
                    str += head.NextCup.CupLabel + " ";
                    str += head.NextCup.NextCup.CupLabel;
                }
                else
                {
                    int start = head.CupLabel;
                    while (head.NextCup.CupLabel != start)
                    {
                        str += $"{head.NextCup.CupLabel}";
                        head = head.NextCup;
                    }
                }
                return str;
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
}
