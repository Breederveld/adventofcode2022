using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;
using static System.Net.Mime.MediaTypeNames;

namespace Puzzle_2022_24_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rootFolder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var input = File.ReadAllText(Path.Combine(rootFolder, "input.txt"));

            var sw = new Stopwatch();
            var strings = input.Trim().Split("\n").Select(s => s.TrimEnd()).ToArray();
            //var groups = input.Trim().Split("\n\n").Select(grp => grp.Split("\n").ToArray()).ToArray();
            //var ints = strings.Where(st => !string.IsNullOrWhiteSpace(st)).Select(st => int.Parse(st)).ToArray();
            sw.Start();

            var width = strings[0].Length - 2;
            var height = strings.Length - 2;
            var blizzards = strings.Skip(1).Take(height)
                .SelectMany((s, y) => s.Skip(1).Take(width).Select((d, x) => (x, y, d)))
                .Where(t => t.d != '.')
                .Select(t =>
                {
                    switch (t.d)
                    {
                        case '<':
                            return (t.x, t.y, t.d, dx: -1, dy: 0);
                        case '>':
                            return (t.x, t.y, t.d, dx: 1, dy: 0);
                        case '^':
                            return (t.x, t.y, t.d, dx: 0, dy: -1);
                        case 'v':
                            return (t.x, t.y, t.d, dx: 0, dy: 1);
                        default:
                            return (t.x, t.y, t.d, dx: 0, dy: 0);
                    }
                })
                .ToArray();
            var start = (x: strings.First().IndexOf('.') - 1, y: 0);
            var end = (x: strings.Last().IndexOf('.') - 1, y: height - 1);

            var rounds = 1000;
            var grids = new bool[rounds, width, height];
            for (var round = 1; round < rounds; round++)
            {
                var next = blizzards
                    .Select(t =>
                    {
                        var x = t.x + t.dx;
                        var y = t.y + t.dy;
                        if (t.dx == 0)
                        {
                            if (y < 0)
                            {
                                y = height - 1;
                            }
                            else if (y == height)
                            {
                                y = 0;
                            }
                        }
                        else if (t.dy == 0)
                        {
                            if (x < 0)
                            {
                                x = width - 1;
                            }
                            else if (x == width)
                            {
                                x = 0;
                            }
                        }
                        return (x, y, t.d, t.dx, t.dy);
                    })
                    .ToArray();
                blizzards = next;
                foreach (var blizzard in blizzards)
                {
                    grids[round, blizzard.x, blizzard.y] = true;
                }
            }

            var grid = new bool[width, height];
            grid[start.x, start.y] = true;
            var moves = new[] { (x: 0, y: -1), (x: 0, y: 1), (x: -1, y: 0), (x: 1, y: 0) };
            var result = 1;
            for (; result < rounds && !grid[end.x, end.y]; result++)
            {
                var newGrid = new bool[width, height];
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        if (grid[x, y])
                        {
                            newGrid[x, y] = true;
                            continue;
                        }
                        foreach (var move in moves)
                        {
                            var xx = x + move.x;
                            var yy = y + move.y;
                            if (xx < 0 || xx >= width)
                            {
                                continue;
                            }
                            if (yy < 0 || yy >= height)
                            {
                                continue;
                            }
                            if (grid[xx, yy])
                            {
                                newGrid[x, y] = true;
                                break;
                            }
                        }
                    }
                }
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        if (grids[result, x, y])
                        {
                            newGrid[x, y] = false;
                        }
                    }
                }
                grid = newGrid;
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}