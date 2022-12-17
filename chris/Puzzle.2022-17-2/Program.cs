using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Puzzle_2022_17_2_2022_17_1
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

            var grid = new bool[7, 100000];
            var maxHeight = 0;
            var jetIdx = 0;
            var rows = new List<int>();
            var blockRowHeights = new Dictionary<int, int>();
            var cycleMinLength = 100; // A guess that is not too low to prevent accidental sub-loops being found
            var result = 0d;
            for (var blockCnt = 0; result == 0; blockCnt++)
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
                blockRowHeights[blockCnt] = maxHeight;
                var skip = 100;
                var delay = 1000;
                if (maxHeight > rows.Count + skip + delay)
                {
                    for (int y = rows.Count; y < maxHeight - skip; y++)
                    {
                        var i = 0;
                        for (var x = 0; x < 7; x++)
                        {
                            i = i * 2 + (grid[x, y] ? 1 : 0);
                            //grid[x, y] = false;
                        }
                        rows.Add(i);
                    }

                    // Search for cycles.
                    var cycleTest = rows.Count / 3;
                    if (cycleTest > cycleMinLength)
                    {
                        for (var start = 0; start < cycleTest; start++)
                        {
                            for (var next = start + cycleMinLength; next < start + cycleTest; next++)
                            {
                                if (Enumerable.Range(0, next - start).All(i => rows[start + i] == rows[next + i]))
                                {
                                    for (int off = 0; off < next - start; off++)
                                    {
                                        var startBlock = blockRowHeights.FirstOrDefault(kv => kv.Value == start);
                                        var nextBlock = blockRowHeights.FirstOrDefault(kv => kv.Value == next);
                                        if (startBlock.Key > 0 && nextBlock.Key > 0)
                                        {
                                            // Found valid cycle.
                                            var period = nextBlock.Key - startBlock.Key;
                                            var relStart = 1000000000000 % period - 1;
                                            var relStartBlock = blockRowHeights.FirstOrDefault(kv => kv.Key == relStart);
                                            result = (1000000000000 / period) * (nextBlock.Value - startBlock.Value) + relStartBlock.Value;
                                            // Just break out of any loop asap.
                                            off = 10000;
                                            next = 10000;
                                            start = 5000;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}