using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_07_2
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

            var path = new Stack<string>();
            var dirs = new List<string>();
            var files = new List<(string, string, int)>();
            foreach (var line in strings)
            {
                var parts = line.Split(' ');
                if (parts[0] == "$")
                {
                    switch (parts[1])
                    {
                        case "cd":
                            if (parts[2] == "..")
                                path.Pop();
                            else
                            {
                                path.Push(parts[2]);
                                dirs.Add(string.Join("/", path.Reverse()));
                            }
                            break;
                        case "ls":
                            break;
                    }
                }
                else
                {
                    if (parts[0] == "dir")
                    {

                    }
                    else
                    {
                        files.Add((string.Join("/", path.Reverse()), parts[1], int.Parse(parts[0])));
                    }
                }
            }

            var required = 30000000 - (70000000 - files.Sum(t => t.Item3));

            var result = dirs
                .Select(dir => new { dir, sum = files.Where(f => f.Item1.StartsWith(dir)).Sum(t => t.Item3) })
                .Where(o => o.sum >= required)
                .OrderBy(o => o.sum)
                .First().sum;

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}