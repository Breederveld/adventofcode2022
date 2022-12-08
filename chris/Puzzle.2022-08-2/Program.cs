using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_08_2
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
            for (var x = 1; x < w - 1; x++)
            {
                for (var y = 1; y < h - 1; y++)
                {
                    var score = 1;
                    for (var xx = x - 1; xx >= 0; xx--)
                    {
                        if (xx == 0 || ints[y][xx] >= ints[y][x])
                        {
                            score *= x - xx;
                            break;
                        }
                    }
                    for (var xx = x + 1; xx < w; xx++)
                    {
                        if (xx == w - 1 || ints[y][xx] >= ints[y][x])
                        {
                            score *= xx - x;
                            break;
                        }
                    }
                    for (var yy = y - 1; yy >= 0; yy--)
                    {
                        if (yy == 0 || ints[yy][x] >= ints[y][x])
                        {
                            score *= y - yy;
                            break;
                        }
                    }
                    for (var yy = y + 1; yy < h; yy++)
                    {
                        if (yy == h - 1 || ints[yy][x] >= ints[y][x])
                        {
                            score *= yy - y;
                            break;
                        }
                    }
                    if (score > result)
                        result = score;
                }
            }

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}