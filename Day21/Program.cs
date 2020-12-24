//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography.X509Certificates;

//namespace Day21
//{
//    class Program
//    {
//        static Dictionary<string, int> IngredientCounts;
//        static Dictionary<string, HashSet<string>> PossibleAllergens;

//        static void Main(string[] args)
//        {
//            string[] input = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day21/input.txt");

//            Console.WriteLine($"Part 1: {Part1(input)}");
//            Console.WriteLine($"Part 2: {Part2(input)}");
//        }

//        static void ParseInput(string[] input)
//        {
//            IngredientCounts = new Dictionary<string, int>();
//            PossibleAllergens = new Dictionary<string, HashSet<string>>();

//            foreach (string row in input)
//            {
//                string[] line = row.Split(" (contains ");
//                string[] ingredients = line[0].Split(" ");
//                string[] allergens = line[1].Replace(")", "").Split(", ");

//                foreach (string ingredient in ingredients)
//                {
//                    if (IngredientCounts.ContainsKey(ingredient))
//                    {
//                        IngredientCounts[ingredient]++;
//                    }
//                    else IngredientCounts[ingredient] = 1;
//                }

//                foreach (string allergen in allergens)
//                {
//                    if (PossibleAllergens.ContainsKey(allergen))
//                    {
//                        PossibleAllergens[allergen].IntersectWith(ingredients);
//                    }
//                    else PossibleAllergens[allergen] = new HashSet<string>(ingredients);
//                }
//            }
//        }

//        static int Part1(string[] input)
//        {
//            ParseInput(input);
//            int ingredientCount = 0;

//            HashSet<string> allergens = PossibleAllergens.Values.SelectMany(x => x).ToHashSet();
//            ingredientCount = IngredientCounts.Where(keyValuePair => !allergens.Contains(keyValuePair.Key)).Sum(keyValuePair => keyValuePair.Value);

//            return ingredientCount;
//        }

//        static string Part2(string[] input)
//        {
//            ParseInput(input);
//            while (PossibleAllergens.Values.Any(x => x.Count != 1))
//            {
//                foreach(string allergen in PossibleAllergens.Keys)
//                {
//                    HashSet<string> potAllergens = PossibleAllergens[allergen];
//                    if (potAllergens.Count != 1) continue;

//                    foreach((string key, HashSet<string> value) in PossibleAllergens)
//                    {
//                        if (key == allergen) continue;

//                        HashSet<string> list = value.Where(x => x != potAllergens.Single()).ToHashSet();
//                        PossibleAllergens[key] = list;

//                    }
//                }
//            }
//            return string.Join(",", PossibleAllergens.OrderBy(x => x.Key).Select(x => x.Value.Single()));
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            var foods = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day21/input.txt");
            var food_ingredients = foods.Select((f, i) => (
                food: i,
                ingredients: f.Split().TakeWhile(i => i != "(contains").ToArray(),
                alergens: f.Split(new char[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries)
                    .SkipWhile(i => i != "contains").Skip(1).ToArray()));

            var by_alergen = food_ingredients.SelectMany(f => f.alergens.Select(a => (a, f))).ToLookup(a => a.a, a => a.f);
            var risky = by_alergen.ToDictionary(g => g.Key, g =>
                g.Skip(1).Aggregate(g.First().ingredients.AsEnumerable(), (i, g2) => i.Intersect(g2.ingredients)).ToList());

            var all_risky = risky.SelectMany(r => r.Value).Distinct().ToHashSet();
            var safe = food_ingredients.SelectMany(f => f.ingredients).Where(i => !all_risky.Contains(i));

            Console.WriteLine($"Part 1: {safe.Count()}");

            while (risky.Any(r => r.Value.Count() > 1))
            {
                var visited = new HashSet<string>();
                while (risky.Any(r => r.Value.Count() == 1 && !visited.Contains(r.Key)))
                {
                    var single = risky.First(g => !visited.Contains(g.Key) && g.Value.Count() == 1);
                    foreach (var s in risky.Where(r => r.Value.Count() > 1))
                    {
                        risky[s.Key].Remove(single.Key);
                    }
                    visited.Add(single.Key);
                }
                visited.Clear();
                while (risky.Any(r => r.Value.Count() > 1 && !visited.Contains(r.Key)))
                {
                    var single = risky.First(g => !visited.Contains(g.Key));
                    var it = single.Value.Where(a => risky.Count(r => r.Value.Contains(a)) == 1);
                    if (it.Any())
                    {
                        risky[single.Key] = it.ToList();
                        if (it.Count() == 1)
                        {
                            foreach (var s in risky.Where(r => r.Value.Count() > 1))
                            {
                                risky[s.Key].Remove(it.First());
                            }
                        }
                    }
                    visited.Add(single.Key);
                }
            }

            var danger = string.Join(",", risky.OrderBy(k => k.Key).Select(a => a.Value.First()));
            Console.WriteLine($"Part 2: {danger}");
        }
    }
}
