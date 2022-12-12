using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Puzzle_2022_12_2
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

            var h = strings.Length;
            var w = strings[0].Length;
            var steps = new int[w, h];
            var dirs = new (int x, int y)[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

            var result = 0;
            var next = new Queue<(int x, int y)>();
            var ends = new List<(int x, int y)>();
            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    var c = strings[y][x];
                    if (c == 'E')
                    {
                        next.Enqueue((x, y));
                        steps[x, y] = 0;
                    }
                    else
                    {
                        steps[x, y] = 10000;
                    }
                    if (c == 'a')
                    {
                        ends.Add((x, y));
                    }
                }
            }

            while (next.Count > 0)
            {
                (var x, var y) = next.Dequeue();
                var c = strings[y][x];
                if (c == 'E')
                    c = 'z';
                foreach (var dir in dirs)
                {
                    var xx = x + dir.x;
                    var yy = y + dir.y;
                    if (xx >= 0 && yy >= 0 && xx < w && yy < h && steps[xx, yy] > steps[x, y] + 1)
                    {
                        var cc = strings[yy][xx];
                        if (cc == 'S')
                            cc = 'a';
                        if (cc >= c - 1)
                        {
                            steps[xx, yy] = steps[x, y] + 1;
                            next.Enqueue((xx, yy));
                        }
                    }
                }
            }

            result = ends.Select(t => steps[t.x, t.y]).Min();
            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }

    public class Monkey
    {
        public List<long> Items { get; set; }
        public Func<long, long> Op { get; set; }
        public Func<long, int> Test { get; set; }
        public long Inspected { get; set; }
    }
}