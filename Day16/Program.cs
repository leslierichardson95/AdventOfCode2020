using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    internal class Field
    {
        public string name;
        public int lower;
        public int upper;
        public int? lower2 = null;
        public int? upper2 = null;
    }

    class Program
    {
        static List<Field> validFields = new List<Field>();
        static List<string> tickets;
        static List<string> validTickets;
        static string myTicket;

        static void Main(string[] args)
        {
            string text = File.ReadAllText("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day16/input.txt");

            string[] firstSplit = text.Split("\n\n");

            foreach (string line in firstSplit[0].Split('\n'))
            {
                Field ticketField = new Field();
                var secondsplit = line.Split(':');
                ticketField.name = secondsplit[0];

                var thirdSplit = secondsplit[1].Split("- or".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (thirdSplit.Length == 4)
                {
                    ticketField.lower = int.Parse(thirdSplit[0]);
                    ticketField.upper = int.Parse(thirdSplit[1]);
                    ticketField.lower2 = int.Parse(thirdSplit[2]);
                    ticketField.upper2 = int.Parse(thirdSplit[3]);
                }

                validFields.Add(ticketField);

            }

            myTicket = firstSplit[1].Split('\n')[1];

            tickets = new List<string>(firstSplit[2].Split('\n'));
            tickets.RemoveAt(0);
            validTickets = new List<string>(tickets);

            Console.WriteLine($"Ticket Error Rate: {GetErrorRate()}");
            Console.WriteLine($"Departure Field Match Product: {MatchFieldsProduct()}");

        }

        static int GetErrorRate()
        {
            int errorRate = 0;

            foreach (string ticket in tickets)
            {
                List<string> strs = ticket.Split(",").ToList();
                List<int> ticketNumbers = new List<int>();
                foreach (string str in strs)
                {
                    ticketNumbers.Add(int.Parse(str));
                }

                foreach (int ticketNumber in ticketNumbers)
                {
                    bool isValid = false;
                    foreach (Field validField in validFields)
                    {
                        if ((ticketNumber >= validField.lower && ticketNumber <= validField.upper) ||
                            (ticketNumber >= validField.lower2 && ticketNumber <= validField.upper2))
                        {
                            isValid = true;
                            break;
                        }
                    }
                    if (!isValid)
                    {
                        errorRate += ticketNumber;
                        validTickets.Remove(ticket);
                    }
                }
            }
            return errorRate;
        }

        static long MatchFieldsProduct()
        {
            Dictionary<int, string> KnownFields = new Dictionary<int, string>();
            Dictionary<(int ticketPosition, string name), int> TicketsThatMatch = new Dictionary<(int ticketPosition, string name), int>();

            int t = 0;
            while (t < validTickets.Count) 
            {
                List<string> strs = validTickets[t].Split(",").ToList();
                List<int> tFields = new List<int>();
                foreach (string str in strs)
                {
                    tFields.Add(int.Parse(str));
                }

                for (int i = 0; i < tFields.Count; i++)
                {
                    var field = tFields[i];
                    for (int j = 0; j < validFields.Count; j++)
                    {
                        var v = validFields[j];
                        if (((v.lower <= field && field <= v.upper) || (v.lower2 <= field && field <= v.upper2)))
                        {
                            if (!TicketsThatMatch.ContainsKey((i, v.name))) TicketsThatMatch[(i, v.name)] = 1;
                            else TicketsThatMatch[(i, v.name)]++;
                        }
                    }
                }
                t++;
            }

            while (KnownFields.Count < validFields.Count)
            {
                for (int i = 0; i < validFields.Count; i++)
                {
                    var ValidAtPosition = TicketsThatMatch.Where(x => x.Key.ticketPosition == i).ToList();

                    if (ValidAtPosition.Count(x => x.Value == validTickets.Count) == 1)
                    {
                        var tmp = ValidAtPosition.First(x => x.Value == validTickets.Count);
                        KnownFields[tmp.Key.ticketPosition] = tmp.Key.name;

                        foreach (var k in TicketsThatMatch.Keys)
                        {
                            if (k.name == tmp.Key.name) TicketsThatMatch.Remove(k);
                        }

                    }
                }
            }

            List<string> fieldStrs = myTicket.Split(",").ToList();
            List<int> myTicketFields = new List<int>();
            foreach (string fieldStr in fieldStrs)
            {
                myTicketFields.Add(int.Parse(fieldStr));
            }

            long departureFieldsProduct = 1;

            foreach (var f in KnownFields)
            {
                if (f.Value.Contains("departure")) departureFieldsProduct *= myTicketFields[f.Key];
            }

            return departureFieldsProduct;
        }
    }
}
