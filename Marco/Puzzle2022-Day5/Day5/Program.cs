using System;
using System.Collections.Generic;
using System.Linq;

namespace Day5
{
    class Program
    {
        public static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("C:/Visual Studio Projects/AoC2022/Day5/PuzzleInput/TextFile1.txt");

            var splitStackFromMoves = input.Split("\r\n\r\n");
            var stacks = splitStackFromMoves[0];
            var inputMovesString = splitStackFromMoves[1];
            var movesPerLine = inputMovesString.Split("\r\n");
            var moves = movesPerLine
                .Select(e => e.Replace("move ", "")
                .Replace(" from ", ",")
                .Replace(" to ", ","))
                .Select(e => e.Split(","));
            var crateStacks = FillStacks(stacks);

            foreach(var move in moves)
            {
                for (int i =0; i<int.Parse(move[0]); i++)
                {
                    var popItem = crateStacks[int.Parse(move[1])-1].Pop();
                    crateStacks[int.Parse(move[2])-1].Push(popItem);
                }  
            }
            PuzzleTwo(moves, stacks);
        }

        public static void PuzzleTwo(IEnumerable<string[]> moves, string stacks)
        {
            var crateStacks = FillStacks(stacks);

            foreach (var move in moves)
            {
                List<char> popitems = new List<char>();
                for (int i = 0; i < int.Parse(move[0]); i++)
                {
                    popitems.Add(crateStacks[int.Parse(move[1]) - 1].Pop());
                }
                popitems.Reverse();
                foreach(var item in popitems)
                {
                    crateStacks[int.Parse(move[2]) - 1].Push(item);
                }
                popitems.Clear();
            }
        }

        public static Stack<char>[] FillStacks(string stacks)
        {
            var tempStack = Enumerable.Range(0, 9).Select(e => new Stack<char>()).ToArray();
            var crateStacks = Enumerable.Range(0, 9).Select(e => new Stack<char>()).ToArray();
            var rows = stacks.Split("\r\n").Take(8);

            foreach (var row in rows)
            {
                var positions = row.Replace("   ", "!").Replace(" ", "").Replace("[", "").Replace("]", "").Replace("!!!!", "!!!");
                var counter = 0;
                foreach (var crate in positions)
                {
                    if (!crate.ToString().Equals("!"))
                    {
                        tempStack[counter].Push(crate);
                    }
                    counter++;
                }
            }
            var cnt = 0;
            foreach(var tempCrate in tempStack)
            { 
                while(tempCrate.Count() != 0)
                {
                    crateStacks[cnt].Push(tempCrate.Pop());
                }
                cnt++;
            }

            return crateStacks;
        }
    }
}
