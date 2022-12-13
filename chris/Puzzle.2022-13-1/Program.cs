using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MoreLinq;

namespace Puzzle_2022_13_1
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

            var result = strings.Batch(3)
                .Select((b, i) => (Compare(b.First(), b.Skip(1).First()), i + 1))
                .Where(t => t.Item1)
                .Select(t => t.Item2)
                .Sum();

            sw.Stop();
            Console.WriteLine(result);
            Console.WriteLine($"Took {sw.Elapsed}");
            await Task.FromResult(0);
        }

        private static bool Compare(string l, string r)
        {
            var depth = 0;
            var lidx = 0;
            var ridx = 0;
            while (lidx < l.Length && ridx < r.Length)
            {
                var lc = l[lidx];
                var rc = r[ridx];

                // Check for integers.
                if (lc >= '0' && lc <= '9' && rc >= '0' && rc <= '9')
                {
                    var li = 0;
                    var ri = 0;
                    while (lidx < l.Length && l[lidx] >= '0' && l[lidx] <= '9')
                    {
                        li = li * 10 + l[lidx] - '0';
                        lidx++;
                    }
                    while (ridx < r.Length && r[ridx] >= '0' && r[ridx] <= '9')
                    {
                        ri = ri * 10 + r[ridx] - '0';
                        ridx++;
                    }
                    if (li > ri)
                        return false;
                    if (li < ri)
                        return true;
                    continue;
                }

                // Check for array endings.
                if (lc == ']')
                {
                    lidx++;
                    if (rc == ']')
                    {
                        if (depth > 0)
                            return false;
                        ridx++;
                        continue;
                    }
                    return true;
                }
                if (rc == ']')
                {
                    if (depth <= 0)
                        return false;
                    ridx++;
                    depth++;
                    continue;
                }

                // Check for array starts.
                if (lc == '[')
                {
                    lidx++;
                    if (rc == '[')
                    {
                        ridx++;
                    }
                    else
                    {
                        depth++;
                    }
                    continue;
                }
                else if (rc == '[')
                {
                    ridx++;
                    depth--;
                    continue;
                }

                // Go to next list item.
                if (lc == ',' && rc == ',')
                {
                    lidx++;
                    while (depth < 0)
                    {
                        if (r[ridx] == '[')
                            depth--;
                        else if (r[ridx] == ']')
                            depth++;
                        ridx++;
                    }
                    if (depth > 0)
                        return false;
                    if (r[ridx] == ',')
                        ridx++;
                }
            }
            return false;
        }
    }
}