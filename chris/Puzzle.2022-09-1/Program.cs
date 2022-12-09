using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_09_1
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
            //var ints = strings.Where(st => !string.IsNullOrWhiteSpace(st)).Select(st => st.Select(c => c - '0').ToArray()).ToArray();
            sw.Start();

            var motions = strings.SelectMany(s => new string(s.Split(' ')[0][0], int.Parse(s.Split(' ')[1]))).ToArray();
            var head = (0, 0);
            var tail = (0, 0);
            var positions = new List<(int, int)>();

            foreach (var motion in motions)
            {
                switch (motion)
                {
                    case 'U':
                        head = (head.Item1, head.Item2 - 1);
                        break;
                    case 'D':
                        head = (head.Item1, head.Item2 + 1);
                        break;
                    case 'L':
                        head = (head.Item1 - 1, head.Item2);
                        break;
                    case 'R':
                        head = (head.Item1 + 1, head.Item2);
                        break;
                }
                var dx = Math.Abs(tail.Item1 - head.Item1);
                var dy = Math.Abs(tail.Item2 - head.Item2);
                if ((dx >= 1 && dy > 1) || dx > 1 && dy >= 1)
                {
                    tail = (tail.Item1 + Math.Sign(head.Item1 - tail.Item1), tail.Item2 + Math.Sign(head.Item2 - tail.Item2));
                }
                else if (dx > 1)
                {
                    tail = (tail.Item1 + Math.Sign(head.Item1 - tail.Item1), tail.Item2);
                }
                else if (dy > 1)
                {
                    tail = (tail.Item1, tail.Item2 + Math.Sign(head.Item2 - tail.Item2));
                }
                positions.Add(tail);
            }

            var result = positions.Distinct().Count();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}
