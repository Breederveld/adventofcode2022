using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2022_20_1
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
            var ints = strings.Where(st => !string.IsNullOrWhiteSpace(st)).Select(st => int.Parse(st)).ToList();
            sw.Start();

            var mix = new List<int>(Enumerable.Range(0, ints.Count));
            for (var iidx = 0; iidx < ints.Count; iidx++)
            {
                var i = ints[iidx];
                var idx = mix.IndexOf(iidx);
                var newIdx = (idx + i + (ints.Count - 1) * 10) % (ints.Count - 1);
                if (idx < newIdx)
                {
                    newIdx++;
                }
                else
                {
                    idx++;
                }
                mix.Insert(newIdx, iidx);
                mix.RemoveAt(idx);
                var dbg = mix.Select(x => ints[x]).ToArray();          
            }
            var zidx = ints.IndexOf(0);
            zidx = mix.IndexOf(zidx);
            var result = Enumerable.Range(1, 3).Sum(i => ints[mix[(zidx + i * 1000) % ints.Count]]);

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}