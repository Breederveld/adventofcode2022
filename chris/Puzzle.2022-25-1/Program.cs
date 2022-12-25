using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_25_1
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

            var snafu = "=-012".Select((c, idx) => (c, idx)).ToDictionary(t => t.c, t => (long)(t.idx - 2));
            var rsnafu = snafu.ToDictionary(kv => kv.Value, kv => kv.Key);
            var sum = strings
                .Select((s, idx) => s.Select((c, i) => snafu[c] * (long)Math.Pow(5, s.Length - i - 1)).Sum())
                .Sum();
            var len = (int)Math.Ceiling(Math.Log(sum) / Math.Log(5));
            var result = "";

            while (sum > 0)
            {
                var n = ((int)(sum % 5) + 2) % 5 - 2;
                result = rsnafu[n] + result;
                sum = (sum - n) / 5;
            }
            result = result.TrimStart('0');

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}
