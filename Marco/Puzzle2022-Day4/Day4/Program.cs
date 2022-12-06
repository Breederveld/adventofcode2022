using System;
using System.Collections.Generic;
using System.Linq;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("C:/Visual Studio Projects/AoC2022/Day4/PuzzleInput/TextFile1.txt");
            var fullyContainedCounter = 0;
            foreach (var pair in input)
            {
                var assignment = pair.Split(",");
                var leftRange = assignment[0].Split("-");
                var rightRange = assignment[1].Split("-");

                if ((int.Parse(leftRange[0]) >= int.Parse(rightRange[0])) && (int.Parse(leftRange[1]) <= int.Parse(rightRange[1])))
                {
                    fullyContainedCounter++;
                }
                else if ((int.Parse(leftRange[0]) <= int.Parse(rightRange[0])) && (int.Parse(leftRange[1]) >= int.Parse(rightRange[1])))
                {
                    fullyContainedCounter++;
                }
            }
            Console.WriteLine(fullyContainedCounter + " assignments fully contained by eachother");

            PuzzlePartTwo(input);
        }

        private static void PuzzlePartTwo(string[] input)
        {
            var overlapCounter = 0;
            foreach (var pair in input)
            {
                var assignment = pair.Split(",");
                var leftRange = FillArray(assignment[0]);
                var rightRange = FillArray(assignment[1]);

                var matches = leftRange.Where(e => rightRange.Contains(e));
                if (matches.Any())
                {
                    overlapCounter++;
                }
            }

            Console.WriteLine("Amount of overlaps: " + overlapCounter);
        }

        private static int[] FillArray(string assignment)
        {
            var range = assignment.Split("-");
            List<int> returnList = new List<int>();
            var counter = int.Parse(range[0]);
            returnList.Add(int.Parse(range[0]));
            returnList.Add(int.Parse(range[1]));
            counter++;

            while (counter < int.Parse(range[1]))
            {
                returnList.Add(counter);
                counter++;
            }

            return returnList.ToArray();
        }
    }
}
