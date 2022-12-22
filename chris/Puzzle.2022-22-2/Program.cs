using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2022_22_2
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

            var side = height / 4;
            Func<int, int, int> getFace = (x, y) =>
            {
                if (y < side)
                {
                    if (x < side * 2)
                        return 1;
                    return 2;
                }
                if (y < side * 2)
                    return 3;
                if (y < side * 3)
                {
                    if (x < side)
                        return 4;
                    return 5;
                }
                return 6;
            };
            var turn = new Func<Direction, int, Direction>((dir, i) => (Direction)(((int)direction + i) % 4));
            foreach (var step in path)
            {
                if (step is string dir)
                {
                    switch (dir)
                    {
                        case "L":
                            direction = turn(direction, 3);
                            break;
                        case "R":
                            direction = turn(direction, 1);
                            break;
                    }
                }
                else if (step is int len)
                {
                    for (int i = 0; i < len; i++)
                    {
                        (int x, int y) move = direction == Direction.Right ? (1, 0) : direction == Direction.Down ? (0, 1) : direction == Direction.Left ? (-1, 0) : (0, -1);
                        (var x, var y) = (pos.x + move.x, pos.y + move.y);
                        if (direction == Direction.Left && (x < 0 || map[y][x] == ' '))
                        {
                            switch (getFace(pos.x, pos.y))
                            {
                                case 1:
                                    y = side * 3 - pos.y - 1;
                                    x = 0;
                                    direction = turn(direction, 2);
                                    break;
                                case 2:
                                    break;
                                case 3:
                                    x = pos.y - side;
                                    y = side * 2;
                                    direction = turn(direction, 3);
                                    break;
                                case 4:
                                    y = side * 3 - pos.y - 1;
                                    x = side;
                                    direction = turn(direction, 2);
                                    break;
                                case 5:
                                    break;
                                case 6:
                                    x = pos.y - side * 2;
                                    y = 0;
                                    direction = turn(direction, 3);
                                    break;
                            }
                        }
                        else if (direction == Direction.Right && (x >= map[y].Length || map[y][x] == ' '))
                        {
                            switch (getFace(pos.x, pos.y))
                            {
                                case 1:
                                    break;
                                case 2:
                                    y = side * 3 - pos.y - 1;
                                    x = side * 2 - 1;
                                    direction = turn(direction, 2);
                                    break;
                                case 3:
                                    x = side + pos.y;
                                    y = side - 1;
                                    direction = turn(direction, 3);
                                    break;
                                case 4:
                                    break;
                                case 5:
                                    y = side * 3 - pos.y - 1;
                                    x = side * 3 - 1;
                                    direction = turn(direction, 2);
                                    break;
                                case 6:
                                    x = pos.y - side * 2;
                                    y = side * 3 - 1;
                                    direction = turn(direction, 3);
                                    break;
                            }
                        }
                        else if (direction == Direction.Up && (y < 0 || map[y][x] == ' '))
                        {
                            switch (getFace(pos.x, pos.y))
                            {
                                case 1:
                                    y = side * 2 + pos.x;
                                    x = 0;
                                    direction = turn(direction, 1);
                                    break;
                                case 2:
                                    x = pos.x - side * 2;
                                    y = side * 4 - 1;
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    y = side + pos.x;
                                    x = side;
                                    direction = turn(direction, 1);
                                    break;
                                case 5:
                                    break;
                                case 6:
                                    break;
                            }
                        }
                        else if (direction == Direction.Down && (y >= map.Length || map[y][x] == ' '))
                        {
                            switch (getFace(pos.x, pos.y))
                            {
                                case 1:
                                    break;
                                case 2:
                                    y = pos.x - side;
                                    x = side * 2 - 1;
                                    direction = turn(direction, 1);
                                    break;
                                case 3:
                                    break;
                                case 4:
                                    break;
                                case 5:
                                    y = pos.x + side * 2;
                                    x = side - 1;
                                    direction = turn(direction, 1);
                                    break;
                                case 6:
                                    x = pos.x + side * 2;
                                    y = 0;
                                    break;
                            }
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
