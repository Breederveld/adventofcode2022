using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using MoreLinq;

namespace Puzzle_2022_10_2
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

            var x = 1;
            var xes = new List<int>();

            for (int c = 0; c < 220; c++)
            {
                xes.Add(x);
                if (strings.Length <= c || strings[c] == "noop")
                {

                }
                else
                {
                    xes.Add(x);
                    x += int.Parse(strings[c].Substring(5));
                }
            }
            var result = string.Join("\n", Enumerable.Range(0, 240)
                .Select(c => c % 40 >= xes[c] - 1 && c % 40 <= xes[c] + 1 ? '#' : '.')
                .Batch(40)
                .Select(b => new string(b.ToArray())));

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}