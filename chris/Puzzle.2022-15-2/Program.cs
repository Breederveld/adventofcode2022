using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CodeTech.Core.Mathematics.Geometry;

namespace Puzzle_2022_15_2
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

            var search = new Regex(@"Sensor at x=(?<xs>\d+), y=(?<ys>\d+): closest beacon is at x=(?<xb>[\d-]+), y=(?<yb>[\d-]+)");
            var pairs = strings
                .Select(s =>
                {
                    var match = search.Match(s);
                    var xs = double.Parse(match.Groups["xs"].Value);
                    var ys = double.Parse(match.Groups["ys"].Value);
                    var xb = double.Parse(match.Groups["xb"].Value);
                    var yb = double.Parse(match.Groups["yb"].Value);
                    var dist = Math.Abs(xs - xb) + Math.Abs(ys - yb);
                    return (xs, ys, xb, yb, dist);
                })
                .ToArray();

            var inRange = new Func<(double x, double y), (double xs, double ys, double xb, double yb, double dist), bool>((pos, pair) =>
            {
                var pDist = Math.Abs(pair.xs - pos.x) + Math.Abs(pair.ys - pos.y);
                return pDist <= pair.dist;
            });
            var result = 0d;

            for (var y = 0; y < 4000000; y++)
            {
                var ranges = pairs
                    .Where(p => p.ys - p.dist <= y && p.ys + p.dist >= y)
                    .Select(p =>
                    {
                        var d = p.dist - Math.Abs(p.ys - y);
                        return (p.xs - d, p.xs + d);
                    })
                    .ToArray();
                foreach (var x in ranges.SelectMany(t => new[] { t.Item1 - 1, t.Item2 + 1 }))
                {
                    if (x < 0 || x > 4000000) continue;
                    if (ranges.All(r => x < r.Item1 || x > r.Item2))
                    {
                        if (!pairs.Any(p => (p.xb == x && p.yb == y) || (p.xs == x && p.ys == y)))
                        {
                            result = x * 4000000 + y;
                            break;
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