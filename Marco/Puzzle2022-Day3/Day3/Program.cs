using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("C:/Visual Studio Projects/AoC2022/Day3/PuzzleInput/TextFile1.txt");
            Dictionary<string, int> dict = CreatePriorityDict();
            var totalPoints = 0;

            foreach (var inventory in input)
            {
                var leftStorage = inventory.Substring(0, inventory.Length / 2);
                var rightStorage = inventory.Substring(inventory.Length / 2);
                List<char> matchedItems = new List<char>();

                foreach (var chr in leftStorage)
                {
                    if (rightStorage.Contains(chr.ToString()))
                    { 
                        if (!matchedItems.Contains(chr))
                        {
                            totalPoints += dict[chr.ToString()];
                            matchedItems.Add(chr);
                        }
                    }
                }
                matchedItems.Clear();
            }
            Console.WriteLine("Total points: " + totalPoints);
            PuzzlePartTwo();
        }

        public static void PuzzlePartTwo()
        {
            var input = System.IO.File.ReadAllLines("C:/Visual Studio Projects/AoC2022/Day3/PuzzleInput/TextFile1.txt");
            List<string> mostCommonItems = new List<string>();
            Dictionary<string, int> dict = CreatePriorityDict();
            var totalPoints = 0;
            var batchesPrGrp = input.Batch(3);

            foreach(var grp in batchesPrGrp)
            {
                var matchCount = 0;
                var point = grp
                    .SelectMany(e => e)
                    .GroupBy(e => e)
                    .OrderByDescending(e => e.Count())
                    .Select(e => e.Key.ToString())
                    .ToArray();

                foreach (var chr in point)
                {
                    var lines = grp.ToArray();
                    var matches = lines.Where(e => e.Contains(chr));

                    if (matches.Count() == 3)
                    {
                        totalPoints += dict[chr];
                    }

                }
            }

            Console.WriteLine("Total points :" + totalPoints);
        }
        public static Dictionary<string, int> CreatePriorityDict()
        {
            Dictionary<string, int> pointDict = new Dictionary<string, int>();
            char[] alphaUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[] alphaLower = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();
            for (int i = 0; i < 26; i++)
            {
                pointDict.Add(alphaLower[i].ToString(), i + 1);
            }
            for (int i = 26; i < 52; i++)
            {
                pointDict.Add(alphaUpper[i - 26].ToString(), i + 1);
            }
            return pointDict;
        }
    }
}
