using System;
using System.Collections.Generic;
using System.Linq;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var caloriesPerElf = System.IO.File.ReadAllText("C:/Visual Studio Projects/AoC2022/Day1/PuzzleInput/InputPuzzle1.txt").Split("\r\n\r\n");
            List<int> sumPerElf = new List<int>();

            foreach (var calorie in caloriesPerElf)
            {            
                var caloriePerElf = calorie.Replace("\r\n", ",").Split(",");
                var sum = int.Parse(caloriePerElf[0]);

                for(int i =1; i< caloriePerElf.Length; i++)
                {
                    sum = sum + int.Parse(caloriePerElf[i]);
                }
                sumPerElf.Add(sum);
            }

            Console.WriteLine("Highest number of calories: " + sumPerElf.Max(e => e));

            //Puzle part Two:
            var topThree = sumPerElf.OrderByDescending(e => e).Take(3).Sum();
            Console.WriteLine("Total Top three: " + topThree);
        }
    }
}
