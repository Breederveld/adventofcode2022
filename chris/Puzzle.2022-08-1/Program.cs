using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_08_1
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
            var ints = strings.Where(st => !string.IsNullOrWhiteSpace(st)).Select(st => st.Select(c => c - '0').ToArray()).ToArray();
            sw.Start();

            var w = ints[0].Length;
            var h = ints.Length;
            var result = 0;
            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var isVisible = true;
                    for (var xx = 0; xx < x; xx++)
                    {
                        if (ints[y][xx] >= ints[y][x])
                            isVisible = false;
                    }
                    if (isVisible)
                    {
                        result++;
                        continue;
                    }
                    isVisible = true;
                    for (var xx = x + 1; xx < w; xx++)
                    {
                        if (ints[y][xx] >= ints[y][x])
                            isVisible = false;
                    }
                    if (isVisible)
                    {
                        result++;
                        continue;
                    }
                    isVisible = true;
                    for (var yy = 0; yy < y; yy++)
                    {
                        if (ints[yy][x] >= ints[y][x])
                            isVisible = false;
                    }
                    if (isVisible)
                    {
                        result++;
                        continue;
                    }
                    isVisible = true;
                    for (var yy = y + 1; yy < h; yy++)
                    {
                        if (ints[yy][x] >= ints[y][x])
                            isVisible = false;
                    }
                    if (isVisible)
                    {
                        result++;
                        continue;
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