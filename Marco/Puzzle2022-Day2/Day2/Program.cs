using System;
using System.Collections.Generic;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllLines("C:/Visual Studio Projects/AoC2022/Day2/PuzzleInput/TextFile1.txt");
            Dictionary<string, int> moveAndPoints = new Dictionary<string, int>();
            var points = 0;
            //win
            moveAndPoints.Add("A Y", 8);
            moveAndPoints.Add("B Z", 9);
            moveAndPoints.Add("C X", 7);
            //Draw
            moveAndPoints.Add("A X", 4);
            moveAndPoints.Add("B Y", 5);
            moveAndPoints.Add("C Z", 6);
            //Loss
            moveAndPoints.Add("A Z", 3);
            moveAndPoints.Add("B X", 1);
            moveAndPoints.Add("C Y", 2);

            foreach (var move in input)
            {
                if (moveAndPoints.ContainsKey(move))
                {
                    points = points + moveAndPoints[move];
                }
            }

            Console.WriteLine("Total points = " + points);

            //Part two
            Dictionary<string, int> oppMoveValue = new Dictionary<string, int>();
            points = 0;
            //Lose
            oppMoveValue.Add("A X", 3);
            oppMoveValue.Add("B X", 1);
            oppMoveValue.Add("C X", 2);
            //Draw
            oppMoveValue.Add("A Y", 4);
            oppMoveValue.Add("B Y", 5);
            oppMoveValue.Add("C Y", 6);
            //Win
            oppMoveValue.Add("A Z", 8);
            oppMoveValue.Add("B Z", 9);
            oppMoveValue.Add("C Z", 7);

            foreach (var move in input)
            {
                if (oppMoveValue.ContainsKey(move))
                {
                    points = points + oppMoveValue[move];
                }
            }
            Console.WriteLine("Total points part Two = " + points);

        }
    }
}
