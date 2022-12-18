using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_18_2
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

            var pieces = strings
                .Select(s => s.Split(','))
                .Select(p => (x: int.Parse(p[0]), y: int.Parse(p[1]), z: int.Parse(p[2])))
                .ToArray();
            var maxX = pieces.Max(p => p.x) + 1;
            var maxY = pieces.Max(p => p.y) + 1;
            var maxZ = pieces.Max(p => p.z) + 1;

            var gaps = new List<(int x, int y, int z)>();
            var queue = new Queue<(int x, int y, int z)>();
            queue.Enqueue((0, 0, 0));
            var directions = new (int x, int y, int z)[] { (-1, 0, 0), (1, 0, 0), (0, -1, 0), (0, 1, 0), (0, 0, -1), (0, 0, 1) };
            gaps.Add((0, 0, 0));
            while (queue.Count > 0)
            {
                var curr = queue.Dequeue();
                foreach(var dir in directions)
                {
                    var next = (x: curr.x + dir.x, y: curr.y + dir.y, z: curr.z + dir.z);
                    if (next.x < -1 || next.x > maxX
                        || next.y < -1 || next.y > maxY
                        || next.z < -1 || next.z > maxZ)
                        continue;
                    if (!pieces.Any(p => p.x == next.x && p.y == next.y && p.z == next.z)
                        && !gaps.Any(p => p.x == next.x && p.y == next.y && p.z == next.z))
                    {
                        gaps.Add(next);
                        queue.Enqueue(next);
                    }
                }
            }

            var result = 0;
            for (int i = 0; i < pieces.Length; i++)
            {
                for (int j = 0; j < gaps.Count; j++)
                {
                    var dist = Math.Abs(pieces[i].x - gaps[j].x)
                        + Math.Abs(pieces[i].y - gaps[j].y)
                        + Math.Abs(pieces[i].z - gaps[j].z);
                    if (dist == 1)
                    {
                        result++;
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