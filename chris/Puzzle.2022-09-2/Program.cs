using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_09_2
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
            var knots = new (int, int)[10];
            var positions = new List<(int, int)>();

            foreach (var motion in motions)
            {
                var head = knots[0];
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
                knots[0] = head;
                for (var i = 1; i < knots.Length; i++)
                {
                    var knot = knots[i];
                    head = knots[i - 1];
                    var dx = Math.Abs(knot.Item1 - head.Item1);
                    var dy = Math.Abs(knot.Item2 - head.Item2);
                    var tail = knot;
                    if ((dx >= 1 && dy > 1) || dx > 1 && dy >= 1)
                    {
                        knot = (knot.Item1 + Math.Sign(head.Item1 - knot.Item1), knot.Item2 + Math.Sign(head.Item2 - knot.Item2));
                    }
                    else if (dx > 1)
                    {
                        knot = (knot.Item1 + Math.Sign(head.Item1 - knot.Item1), knot.Item2);
                    }
                    else if (dy > 1)
                    {
                        knot = (knot.Item1, knot.Item2 + Math.Sign(head.Item2 - knot.Item2));
                    }
                    knots[i] = knot;
                }
                positions.Add(knots.Last());
            }

            var result = positions.Distinct().Count();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}
