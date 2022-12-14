using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Puzzle_2022_14_2
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
            maxY += 2;
            for (var x = 0; x < 1000; x++)
            {
                grid[x, maxY] = 1;
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
                grid[sand.x, sand.y] = 2;
                result++;
                if (sand.y == 0)
                    break;
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            PrintGrid(grid);
            await Task.FromResult(0);
        }

        private static void PrintGrid(int[,] grid)
        {
            var minX = grid.GetLength(0) - 1;
            var maxX = 0;
            var minY = grid.GetLength(1) - 1;
            var maxY = 0;
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] == 2)
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }
            minX -= 10;
            minY -= 10;
            if (minY < 0) minY = 0;
            maxX += 10;
            maxY += 10;

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x += 2)
                {
                    var v0 = grid[x, y];
                    var v1 = grid[x - 1, y];
                    var v = v0 == 1 || v1 == 1 ? 1 : v0 == 2 && v1 == 2 ? 2 : v0 == 2 || v1 == 2 ? 3 : 0;
                    Console.Write(v == 0 ? " " : v == 1 ? "#" : v == 2 ? "o" : ".");
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
