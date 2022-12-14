using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_14_1
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

            var grid = new int[1000, 1000];
            var start = (x: 500, y: 0);
            var maxY = 0;
            foreach (var line in strings)
            {
                var lastPoint = (x: -1, y: -1);
                foreach (var point in line.Split(" -> "))
                {
                    var parts = point.Split(',');
                    var x = int.Parse(parts[0]);
                    var y = int.Parse(parts[1]);
                    if (y > maxY) maxY = y;
                    grid[x, y] = 1;

                    if (lastPoint.x != -1)
                    {
                        var steps = Math.Max(Math.Abs(lastPoint.x - x), Math.Abs(lastPoint.y - y));
                        var step = (x: Math.Sign(x - lastPoint.x), y: Math.Sign(y - lastPoint.y));
                        for (var i = 0; i < steps; i++)
                        {
                            lastPoint = (lastPoint.x + step.x, lastPoint.y + step.y);
                            grid[lastPoint.x, lastPoint.y] = 1;
                        }
                    }
                    lastPoint = (x, y);
                }
            }

            var moves = new[] { 0, -1, 1 };
            var result = 0;
            while (true)
            {
                var sand = start;
                var moved = true;
                while (moved && sand.y <= maxY)
                {
                    moved = false;
                    foreach (var move in moves)
                    {
                        if (grid[sand.x + move, sand.y + 1] == 0)
                        {
                            sand = (sand.x + move, sand.y + 1);
                            moved = true;
                            break;
                        }
                    }
                }
                if (sand.y >= maxY)
                    break;
                grid[sand.x, sand.y] = 2;
                result++;
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}