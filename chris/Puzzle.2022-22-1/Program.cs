using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2022_22_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rootFolder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var input = File.ReadAllText(Path.Combine(rootFolder, "input.txt"));

            var sw = new Stopwatch();
            var strings = input.Split("\n").Select(s => s).ToArray();
            //var groups = input.Trim().Split("\n\n").Select(grp => grp.Split("\n").ToArray()).ToArray();
            //var ints = strings.Where(st => !string.IsNullOrWhiteSpace(st)).Select(st => int.Parse(st)).ToArray();
            sw.Start();

            var height = strings.Select((s, i) => (s, i)).First(t => t.s.Length == 0).i;
            var width = strings.Take(height).Max(s => s.Length);
            var map = strings
                .Take(height)
                .Select(s => s.PadRight(width, ' ').ToArray())
                .ToArray();
            var path = Regex.Matches(strings[height + 1], @"(\d+)|\w").Select(m => m.Value)
                .Select(s => int.TryParse(s, out var v) ? (object)v : s)
                .ToArray();
            var direction = Direction.Right;
            var pos = (x: strings[0].IndexOf('.'), y: 0);

            foreach (var step in path)
            {
                if (step is string dir)
                {
                    switch (dir)
                    {
                        case "L":
                            direction = (Direction)(((int)direction + 3) % 4);
                            break;
                        case "R":
                            direction = (Direction)(((int)direction + 1) % 4);
                            break;
                    }
                }
                else if (step is int len)
                {
                    (int x, int y) move = direction == Direction.Right ? (1, 0) : direction == Direction.Down ? (0, 1) : direction == Direction.Left ? (-1, 0) : (0, -1);
                    for (int i = 0; i < len; i++)
                    {
                        (var x, var y) = (pos.x + move.x, pos.y + move.y);
                        if (direction == Direction.Left && (x < 0 || map[y][x] == ' '))
                        {
                            x = map[y].Select((c, idx) => (c, idx)).Last(t => t.c != ' ').idx;
                        }
                        else if (direction == Direction.Right && (x >= map[y].Length || map[y][x] == ' '))
                        {
                            x = map[y].Select((c, idx) => (c, idx)).First(t => t.c != ' ').idx;
                        }
                        else if (direction == Direction.Up && (y < 0 || map[y][x] == ' '))
                        {
                            y = map.Select((s, idx) => (s, idx)).Last(t => t.s[x] != ' ').idx;
                        }
                        else if (direction == Direction.Down && (y >= map.Length || map[y][x] == ' '))
                        {
                            y = map.Select((s, idx) => (s, idx)).First(t => t.s[x] != ' ').idx;
                        }
                        if (map[y][x] == '#')
                        {
                            break;
                        }
                        map[pos.y][pos.x] = direction.ToString()[0];
                        pos = (x, y);
                    }
                }
            }
            map[pos.y][pos.x] = 'X';

            foreach (var line in map)
            {
                Console.WriteLine(line);
            }

            var result = 1000 * (pos.y + 1) + 4 * (pos.x + 1) + (int)direction;

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private enum Direction
        {
            Right,
            Down,
            Left,
            Up,
        }
    }
}