using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2022_05_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rootFolder = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            var input = File.ReadAllText(Path.Combine(rootFolder, "input.txt"));

            var sw = new Stopwatch();
            var strings = input.Split("\n").Select(s => s.TrimEnd()).ToArray();
            //var groups = input.Trim().Split("\n\n").Select(grp => grp.Split("\n").ToArray()).ToArray();
            //var ints = strings.Where(st => !string.IsNullOrWhiteSpace(st)).Select(st => int.Parse(st)).ToArray();
            sw.Start();

            var queues = Enumerable.Range(0, 9)
                .Select(x => new Stack<char>(Enumerable.Range(0, 8).Select(y => strings[y][x * 4 + 1]).Where(c => c != ' ').Reverse()))
                .ToArray();
            var reg = new Regex(@"move (?<move>\d+) from (?<from>\d+) to (?<to>\d+)");
            var operations = strings
                .Skip(10)
                .Select(s => reg.Match(s))
                .Where(m => m.Success)
                .Select(m => (move: int.Parse(m.Groups["move"].Value), from: int.Parse(m.Groups["from"].Value), to: int.Parse(m.Groups["to"].Value)))
                .ToArray();

            foreach (var op in operations)
            {
                var from = queues[op.from - 1];
                var to = queues[op.to - 1];
                for (var i = 0; i < op.move; i++)
                {
                    to.Push(from.Pop());
                }
            }

            var result = string.Join("", queues.Select(q => q.Pop()));

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}