using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Puzzle_2022_02_1
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

            var result = strings
                .Select(s =>
                {
                    var parts = s.Split(' ');
                    return GetScore(parts[0][0], parts[1][0]);
                })
                .Sum();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private static int GetScore(char left, char right)
        {
            var diff =  (3 + (right - 'X') - (left - 'A')) % 3;
            var outcome = diff == 0 ? 3 : diff == 1 ? 6 : 0;
            var value = (right - 'X' + 1);
            return value + outcome;
        }
    }
}