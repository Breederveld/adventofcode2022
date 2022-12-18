using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_18_1
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
            var touching = 0;
            for (int i = 0; i < pieces.Length; i++)
            {
                for (int j = 0; j < pieces.Length; j++)
                {
                    var dist = Math.Abs(pieces[i].x - pieces[j].x)
                        + Math.Abs(pieces[i].y - pieces[j].y)
                        + Math.Abs(pieces[i].z - pieces[j].z);
                    if (dist == 1)
                        touching++;
                }
            }
            var result = pieces.Length * 6 - touching;

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }
    }
}