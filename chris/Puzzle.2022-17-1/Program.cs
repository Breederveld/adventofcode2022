using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_17_1
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

            var blocks = new (int x, int y)[][]
            {
                new[] { (0, 0), (1, 0), (2, 0), (3, 0) },
                new[] { (1, 0), (0, 1), (1, 1), (2, 1), (1, 2) },
                new[] { (2, 0), (2, 1), (0, 2), (1, 2), (2, 2) },
                new[] { (0, 0), (0, 1), (0, 2), (0, 3) },
                new[] { (0, 0), (0, 1), (1, 0), (1, 1) },
            };
            var jets = strings[0];

            var blockWidths = blocks.Select(block => block.Max(t => t.x) + 1).ToArray();
            var blockHeights = blocks.Select(block => block.Max(t => t.y) + 1).ToArray();

            var grid = new bool[7, 10000];
            var maxHeight = 0;
            var jetIdx = 0;
            for (var blockCnt = 0; blockCnt < 2022; blockCnt++)
            {
                var blockIdx = blockCnt % blocks.Length;
                var block = blocks[blockIdx];
                var width = blockWidths[blockIdx];
                var height = blockHeights[blockIdx];
                var blockX = 2;
                var blockY = maxHeight + 3;

                var collision = false;
                var collides = new Func<bool>(() => blockY == -1 || block.Any(t => grid[blockX + t.x, blockY + height - 1 - t.y]));
                while (!collision)
                {
                    if (jets[jetIdx % jets.Length] == '<')
                    {
                        blockX--;
                        if (blockX < 0)
                        {
                            blockX = 0;
                        }
                        if (collides())
                        {
                            blockX++;
                        }
                    }
                    else
                    {
                        blockX++;
                        if (blockX + width >= 7)
                        {
                            blockX = 7 - width;
                        }
                        if (collides())
                        {
                            blockX--;
                        }
                    }
                    jetIdx++;

                    blockY--;
                    collision = collides();
                }
                blockY++;
                foreach (var t in block)
                {
                    grid[blockX + t.x, blockY + height - 1 - t.y] = true;
                }
                maxHeight = Math.Max(maxHeight, blockY + height);
            }
            var result = maxHeight;

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}